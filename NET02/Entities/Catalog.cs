using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NET02.Entities
{
    public class Catalog : IEnumerable
    {
        public List<Book> Books { get; init; }
        
        //IEnumerator returns sorted list of books
        public IEnumerator GetEnumerator()
        {
            var books = Books.OrderBy(b => b.Name);
            return books.GetEnumerator();
        }

        /// <summary>
        /// Indexer:
        ///     returns book by isbn code
        ///     if book with given isbn code doesn't exist - return null;
        /// </summary>
        /// <param name="isbnCode"></param>
        public Book this[string isbnCode]
        {
            get
            {
                return Books.FirstOrDefault(b => b.ISBNCode.Equals(Book.IsbnCodeСorrection(isbnCode)));
            }
        }
        
        public IEnumerable<Book> GetBooks(Author author)
        {
            return Books.Where(b => b.Authors.Any(a=>a.Equals(author)));
        }

        public IEnumerable<Book> GetBooksByDate()
        {
            return Books.OrderByDescending(b => b.PublicationDate);
        }

        /// <summary>
        /// Dictionary keeps Author(Key) and count books by this Author(value)
        /// Authors are Distinct
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetAuthorBooks()
        {
            return Books.SelectMany(b => b.Authors).GroupBy(x=>x.FirstName).Select(x => new {Name = x.Key, Count = x.Count()}).ToList();
        }
    }
}