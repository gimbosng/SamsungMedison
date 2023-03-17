﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// Add
using System.IO;

namespace VideoRental
{
    class Program
    {
        enum Menu
        {
            Main = 0,
            PrintAllVideoTitle,
            Rental,
            Return,
            SaveFile,
            Receipt,
            Exit
        }

        static List<Movie> movieList = new List<Movie>();
        static Dictionary<string, Customer> customerList = new Dictionary<string, Customer>();

        static void Main(string[] args)
        {
            Menu currentMenu = Menu.Main;
            string menu = "", id = "", title = "", periodStr = "", yesNo = "";
            int period = 0;
            int retry = 0, retryMax = 3;
            bool backMain = false;

            movieList.Add(new Movie( "일반1", Movie.REGULAR ));
            movieList.Add(new Movie( "일반2", Movie.REGULAR ));
            movieList.Add(new Movie( "신작1", Movie.NEW_RELEASE ));
            movieList.Add(new Movie( "신작2",Movie.NEW_RELEASE ));
            movieList.Add(new Movie( "어린이1", Movie.CHILDRENS ));
            movieList.Add(new Movie( "어린이2", Movie.CHILDRENS ));

            do
            {
                switch (currentMenu)
                {
                    case Menu.Main:
                        {
                            Console.WriteLine(printMainView());
                            Console.Write("Select Menu: ");
                            menu = Console.ReadLine();
                            Console.WriteLine("------------------");
                            currentMenu = (Menu)(Convert.ToInt32(menu) + 1);
                        }
                        break;

                    // Rental 샵에서 보유중인 모든 비디오 정보를 출력
                    case Menu.PrintAllVideoTitle:
                        {
                            Console.WriteLine("----Movie List-----");
                            foreach (Movie m in movieList)
                            {
                                Console.WriteLine(printMovieInfo(m));
                            }
                            Console.WriteLine("------------------");
                            currentMenu = Menu.Main;
                        }
                        break;

                    // Rental 메뉴로 이동
                    case Menu.Rental:
                        {
                            Movie movie = null;
                            retry = 0;
                            backMain = false;

                            Console.WriteLine("-----Rental Menu-----");
                            Console.Write("Input customer ID :");
                            id = Console.ReadLine();
                            if (!customerList.ContainsKey(id))
                            {
                                Console.Write("입력한 정보와 일치하는 회원이 없습니다. ID를 새로 만드시겠습니까? (Y/N) : ");
                                yesNo = Console.ReadLine();
                                if (yesNo.ToLower().Trim() == "y")
                                    customerList.Add(id, new Customer(id));
                                else
                                {
                                    Console.WriteLine("------------------");
                                    currentMenu = Menu.Main;
                                    break;
                                }
                            }
                            do
                            {
                                Console.Write("Input Video Title :");
                                title = Console.ReadLine();
                                if (title.ToLower().Trim() == "m")
                                {
                                    backMain = true;
                                    break;
                                }
                                movie = movieList.Find(x => x.getTitle() == title);
                                if (movie != null)
                                    break;
                                else
                                    Console.WriteLine(string.Format("입력한 제목과 일치하는 영화가 없습니다. 다시 입력해주세요. [{0} / {1}] (M 입력 시, Main 화면으로 이동)", retry, retryMax));
                                retry++;
                            } while (retry < retryMax);
                            if (movie != null)
                            {
                                bool enterOK = false;
                                retry = 0;
                                do
                                {
                                    Console.Write("Input Period :");
                                    periodStr = Console.ReadLine();
                                    enterOK = int.TryParse(periodStr, out period);
                                    if (enterOK)
                                        break;
                                    else
                                        Console.WriteLine(string.Format("입력한 숫자가 올바르지 않습니다. 다시 입력해주세요. [{0} / {1}]", retry, retryMax));
                                    retry++;
                                } while (retry < retryMax);
                                if (enterOK)
                                    rentMovie(id, movie, period);
                            }
                            Console.WriteLine("------------------");
                            if (backMain)
                            {
                                currentMenu = Menu.Main;
                            }
                            else
                            {
                                // Y입력 시 Rental 메뉴 반복 N 입력 시 Main Menu로 이동
                                Console.Write("Continue? (Y/N) : ");
                                yesNo = Console.ReadLine();
                                currentMenu = yesNo.ToLower().Contains("y") ? Menu.Rental : Menu.Main;
                            }
                        }
                        break;

                    // Return 메뉴로 이동
                    case Menu.Return:
                        {
                            retry = 0;
                            backMain = false;

                            Console.WriteLine("-----Return Menu-----");
                            Console.Write("Input customer ID :");
                            id = Console.ReadLine();
                            if (!customerList.ContainsKey(id))
                            {
                                Console.WriteLine("입력한 정보와 일치하는 회원이 없습니다.");
                            }
                            else
                            {
                                do
                                {
                                    Console.Write("Input Video Title :");
                                    title = Console.ReadLine();
                                    if (title.ToLower().Trim() == "m")
                                    {
                                        backMain = true;
                                        break;
                                    }

                                    if (returnMovie(id, title))
                                        break;
                                    else
                                        Console.WriteLine(string.Format("입력한 제목의 대여 기록이 없습니다. 다시 입력해주세요. [{0} / {1}] (M 입력 시, Main 화면으로 이동)", retry, retryMax));
                                    retry++;
                                } while (retry < retryMax);

                            }
                            Console.WriteLine("------------------");
                            if (backMain)
                            {
                                currentMenu = Menu.Main;
                            }
                            else
                            {
                                // Y입력 시 Return 메뉴 반복 N 입력 시 Main Menu로 이동
                                Console.Write("Continue? (Y/N) : ");
                                yesNo = Console.ReadLine();
                                currentMenu = yesNo.ToLower().Contains("y") ? Menu.Return : Menu.Main;
                            }
                        }
                        break;

                    // 현재 Rental 한 모든 고객 정보를 영수증 스타일로 파일로 저장
                    case Menu.SaveFile:
                        Console.WriteLine("-----Save Receipt-----");
                        foreach (Customer customer in customerList.Values)
                        {
                            Console.WriteLine(saveReceipt(customer));
                        }
                        Console.WriteLine("------------------");
                        currentMenu = Menu.Main;
                        break;

                    // 고객ID를 입력받아 해당 고객이 대여한 비디오 영수증을 출력
                    case Menu.Receipt:
                        Console.WriteLine("-----Print Receipt-----");
                        Console.Write("Input customer ID :");
                        id = Console.ReadLine();
                        if (!customerList.ContainsKey(id))
                        {
                            Console.WriteLine("입력한 정보와 일치하는 회원이 없습니다.");
                        }
                        else
                        {
                            Console.WriteLine("------------------");
                            Console.WriteLine(printReceipt(customerList[id]));
                            Console.WriteLine("------------------");
                        }
                        Console.WriteLine("------------------");
                        // Y입력 시 Return 메뉴 반복 N 입력 시 Main Menu로 이동
                        Console.Write("Continue? (Y/N) : ");
                        yesNo = Console.ReadLine();
                        currentMenu = yesNo.ToLower().Contains("y") ? Menu.Receipt : Menu.Main;
                        break;
                }

            } while (currentMenu != Menu.Exit);
        }

        static string printMainView()
        {
            StringBuilder sRet = new StringBuilder();
            sRet.AppendLine("-----Main Menu-----");
            sRet.AppendLine("0 : Print All Video Title");
            sRet.AppendLine("1 : Rental");
            sRet.AppendLine("2 : Return");
            sRet.AppendLine("3 : Save to File");
            sRet.AppendLine("4 : Receipt");
            sRet.AppendLine("5 : Exit");
            return sRet.ToString();
        }

        static string printMovieInfo(Movie movie)
        {
            if (movie.getPriceCode() == 0)
                return string.Format("[REGULAR] {0}", movie.getTitle());
            else if (movie.getPriceCode() == 1)
                return string.Format("[NEW_RELEASE] {0}", movie.getTitle());
            else if (movie.getPriceCode() == 2)
                return string.Format("[CHILDRENS] {0}", movie.getTitle());
            else
                return "";
        }

        static void rentMovie(string customerID, Movie movie, int period)
        {
            customerList[customerID].addRental(new Rental(movie, period));
        }

        static bool returnMovie(string customerID, string title)
        {
            return customerList[customerID].removeRental(title);
        }

        static string saveReceipt(Customer customer)
        {
            try
            {
                string fileName = Path.Combine(Directory.GetCurrentDirectory(), "Receipt_" + customer.getName());
                File.WriteAllText(fileName, customer.statement());
                return "Receipt_" + customer.getName() + ".txt";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        static string printReceipt(Customer customer)
        {
            return customer.statement();
        }
    }
}
