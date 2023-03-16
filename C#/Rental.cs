using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoRental
{
  public class Rental
  {
    public Rental(Movie movie, int daysRented)
    {
        rentedMovie = movie;
        nDaysRented = daysRented;
    }

    public int getDaysRented() { return nDaysRented; }
    public Movie getMovie() { return rentedMovie; }

    private Movie rentedMovie;
    private int nDaysRented;
  }
}
