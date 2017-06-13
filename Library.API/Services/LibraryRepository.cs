using Library.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.API.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private List<Author>  Authors;
        private List<Book> Books;

        public LibraryRepository()
        {
            Authors = StubData.GetAuthors().ToList();
            Books = StubData.GetBooks().ToList();
        }

        public void AddAuthor(Author author)
        {
            author.Id = Guid.NewGuid();
            Authors.Add(author);

            // the repository fills the id (instead of using identity columns)
            if (author.Books.Any())
            {
                foreach (var book in author.Books)
                {
                    book.Id = Guid.NewGuid();
                }
            }
        }

        public void AddBookForAuthor(Guid authorId, Book book)
        {
            var author = GetAuthor(authorId);
            if (!author.Any())
            {
                // if there isn't an id filled out (ie: we're not upserting),
                // we should generate one
                if (book.Id == null)
                {
                    book.Id = Guid.NewGuid();
                }
                author.SingleOrDefault().Books.Add(book);
            }
        }

        public bool AuthorExists(Guid authorId)
        {
            return Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            Authors.Remove(author);
        }

        public void DeleteBook(Book book)
        {
            Books.Remove(book);
        }

        public IEnumerable<Author> GetAuthor(Guid authorId)
        {
            return Authors.Where(id => id.Id == authorId);
            //return Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return Authors.OrderBy(a => a.FirstName).ThenBy(a => a.LastName);
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            return Authors.Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateAuthor(Author author)
        {
            // no code in this implementation
        }

        public Book GetBookForAuthor(Guid authorId, Guid bookId)
        {
            IEnumerable<Book> books = GetBooksForAuthor(authorId);

            return books.Where(b=>b.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> GetBooksForAuthor(Guid authorId)
        {
            var books = Authors.Where(author => author.Id == authorId).Select(i =>
            {
                return i.Books;
            }
            ).SingleOrDefault().AsEnumerable();
            return books.OrderBy(b => b.Title).ToList();
        }

        public void UpdateBookForAuthor(Book book)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (true);
        }
    }
}
