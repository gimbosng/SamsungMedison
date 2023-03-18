using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static VideoRental.Movie;

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

            foreach (Rental item in customerRental)
            {
                double thisAmount = 0.0;
                Movie movie = item.getMovie();
                switch (movie.getPriceCode())
                {
                    case Genre.REGULAR:
                        thisAmount += 2.0;
                        if (item.getDaysRented() > 2)
                            thisAmount += (item.getDaysRented() - 2) * 1.5;
                        break;
                    case Genre.NEW_RELEASE:
                        thisAmount += item.getDaysRented() * 3;
                        break;

                    case Genre.CHILDRENS:
                        thisAmount += 1.5;
                        if (item.getDaysRented() > 3)
                            thisAmount += (item.getDaysRented() - 3) * 1.5;
                        break;

                    case Genre.THRILLER:
                        thisAmount += 2.0;
                        if (item.getDaysRented() > 2)
                            thisAmount += (item.getDaysRented() - 2) * 1.5;
                        break;
                }

                // Add frequent renter points
                frequentRenterPoints++;

                // Add bonus for a two day new release rental
                if ((item.getMovie().getPriceCode() == Genre.NEW_RELEASE)
                        && item.getDaysRented() > 1) frequentRenterPoints++;

                // Show figures for this rental
                result.AppendLine("\t" + item.getMovie().getTitle() + "\t" + thisAmount.ToString());
                totalAmount += thisAmount;
            }

            result.AppendLine("Amount owed is " + totalAmount);
            result.AppendLine("You earned " + frequentRenterPoints + " frequent renter points");

            return result.ToString();
        }

        public string newFormat()
        {
            StringBuilder result = new StringBuilder();
            foreach (Rental item in customerRental)
            {
                Movie movie = item.getMovie();
                result.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}", movie.getPriceCode(), movie.getTitle(), item.getDaysRented(), (int)movie.getPriceCode()));
            }
            return result.ToString();
        }

        private string customerName;
        private List<Rental> customerRental = new List<Rental>();
    }
}
