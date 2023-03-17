﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoRental
{
    class Customer
    {
        public Customer(string name)
        {
            customerName = name;
        }

        public void addRental(Rental arg) { customerRental.Add(arg); }
        public string getName() { return customerName; }

        public bool removeRental(string title)
        {
            if (customerRental.Count == 0) return false;
            int index = customerRental.FindIndex(x => x.getMovie().getTitle() == title);
            if (index < 0) return false;
            customerRental.RemoveAt(index);
            return true;
        }
        public string statement()
        {
            double totalAmount = 0.0;
            int frequentRenterPoints = 0;
            StringBuilder result = new StringBuilder();

            result.AppendLine("Rental Record for " + getName());


            IEnumerator<Rental> enumerator = customerRental.GetEnumerator();

            for (; enumerator.MoveNext();)
            {
                double thisAmount = 0.0;
                Rental each = enumerator.Current;

                switch (each.getMovie().getPriceCode())
                {
                    case Movie.REGULAR:
                        thisAmount += 2.0;
                        if (each.getDaysRented() > 2)
                            thisAmount += (each.getDaysRented() - 2) * 1.5;
                        break;
                    case Movie.NEW_RELEASE:
                        thisAmount += each.getDaysRented() * 3;
                        break;

                    case Movie.CHILDRENS:
                        thisAmount += 1.5;
                        if (each.getDaysRented() > 3)
                            thisAmount += (each.getDaysRented() - 3) * 1.5;
                        break;

                    case Movie.THRILLER:
                        thisAmount += 2.0;
                        if (each.getDaysRented() > 2)
                            thisAmount += (each.getDaysRented() - 2) * 1.5;
                        break;
                }

                // Add frequent renter points
                frequentRenterPoints++;

                // Add bonus for a two day new release rental
                if ((each.getMovie().getPriceCode() == Movie.NEW_RELEASE)
                        && each.getDaysRented() > 1) frequentRenterPoints++;

                // Show figures for this rental
                result.AppendLine("\t" + each.getMovie().getTitle() + "\t" + thisAmount.ToString());
                totalAmount += thisAmount;
            }

            result.AppendLine("Amount owed is " + totalAmount);
            result.AppendLine("You earned " + frequentRenterPoints + " frequent renter points");

            return result.ToString();
        }

        public string newFormat()
        {
            StringBuilder result = new StringBuilder();
            IEnumerator<Rental> enumerator = customerRental.GetEnumerator();
            for (; enumerator.MoveNext();)
            {
                Rental each = enumerator.Current;
                Movie movie = each.getMovie();
                switch (movie.getPriceCode())
                {
                    case 0: result.AppendLine(String.Format("REGULAR {0} {1} {2}", movie.getTitle(), each.getDaysRented(), movie.getPriceCode()));  break;
                    case 1: result.AppendLine(String.Format("NEW_RELEASE {0} {1} {2}", movie.getTitle(), each.getDaysRented(), movie.getPriceCode())); break;
                    case 2: result.AppendLine(String.Format("CHILDRENS {0} {1} {2}", movie.getTitle(), each.getDaysRented(), movie.getPriceCode())); break;
                    case 3: result.AppendLine(String.Format("THRILLER {0} {1} {2}", movie.getTitle(), each.getDaysRented(), movie.getPriceCode())); break;
                }
            }
            return result.ToString();
        }

        private string customerName;
        private List<Rental> customerRental = new List<Rental>();
    }
}
