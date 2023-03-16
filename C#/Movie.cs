using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoRental
{
  public class Movie
  {
    public const int CHILDRENS = 2;
    public const int REGULAR = 0;
    public const int NEW_RELEASE = 1;

    public Movie(string title, int priceCode = REGULAR)
    {
        movieTitle = title;
        moviePriceCode = priceCode;
    }

    public int getPriceCode() { return moviePriceCode; }
    public void setPriceCode(int args) { moviePriceCode = args; }
    public string getTitle() { return movieTitle; }

    private string movieTitle;
    int moviePriceCode;
  }
}
