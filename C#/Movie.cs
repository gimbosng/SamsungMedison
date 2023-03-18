using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoRental
{
    public class Movie
    {
        public enum Genre
        {
            REGULAR = 0,
            NEW_RELEASE,
            CHILDRENS,
            THRILLER
        }

        public Movie(string title, Genre priceCode = Genre.REGULAR)
        {
            movieTitle = title;
            moviePriceCode = priceCode;
        }

        public Genre getPriceCode() { return moviePriceCode; }
        public void setPriceCode(Genre args) { moviePriceCode = args; }
        public string getTitle() { return movieTitle; }

        private string movieTitle;
        Genre moviePriceCode;
    }
}
