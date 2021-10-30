using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NET02.Entities;

namespace NET02
{
    public class Program
    {
        public static Catalog InitCatalog()
        {
            Author firstAuthor = new Author
            (
                "Boris",
                "Strugatckiy"
            );
            Author secondAuthor = new Author
            (
                "Arkadiy",
                "Strugatckiy"
            );
            Author thirdAuthor = new Author
            (
                "Ivan",
                "Turgenev"
            );
            Book firstBook = new Book
            (
                "888-8-88-888888-8",
                "Doomed City",
                new List<Author> { firstAuthor, secondAuthor },
                new DateTime(1972, 04, 27)
            );
            Book secondBook = new Book
            (
                "777-7-77-777777-7",
                "Roadside Picnic",
                new List<Author> { firstAuthor, secondAuthor },
                new DateTime(1972, 04, 26)
            );
            Book thirdBook = new Book
            (
                "666-6-66-666666-6",
                "Fathers and children",
                new List<Author> { thirdAuthor },
                new DateTime(1862, 05, 13)
            );
            Catalog newCatalog = new Catalog
            {
                Books = new List<Book> {firstBook, secondBook, thirdBook}
            };
            if (newCatalog["888-8-88-888888-8"].Equals(firstBook))
            {
                Console.WriteLine("True");
            }
            return newCatalog;
        }
        static void Main(string[] args)
        {
            Catalog catalog = InitCatalog();
            
            var books = catalog.GetBooks(new Author("Boris", "Strugatckiy"));
            Console.WriteLine("Get Books: ");
            foreach (var book in books)
            {
                Console.WriteLine(book.Name + " " + book.ISBNCode + " " + book.PublicationDate);
            }

            var booksByDate = catalog.GetBooksByDate();
            Console.WriteLine("Get Books by date: ");
            foreach (var book in booksByDate)
            {
                Console.WriteLine(book.Name + " " + book.ISBNCode + " " + book.PublicationDate);
            }

            Console.WriteLine("Get Author and books count: ");
            var authorBooks = catalog.GetAuthorBooks();
            
            IEnumerator orderedBooks = catalog.GetEnumerator();
            while (orderedBooks.MoveNext())
            {
                if (orderedBooks.Current is Book item) Console.WriteLine(item.Name);
            }
        }
    }
}