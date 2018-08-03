using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBAccess;

namespace BusinessLogic
{
    public class BusinessLogicDBOperations
    {

        public List<Book> RetrieveBooksList()
        {
            List<Book> books = null;

            BookCheckInCheckOutDBOperations db = BookCheckInCheckOutDBOperations.getInstance();

            IDataReader reader = db.RetrieveBooksList();

            //DataTable dt = SchemaInfo.CreateBookDetailsSchemaTable();

            if (reader != null)
            {
                books = new List<Book>();

                while (reader.Read())
                {


                    books.Add(new Book
                    {
                        BookID = (int)reader["BookID"],
                        Title = (string)reader["Title"],
                        ISBN = (string)reader["ISBN"],
                        PublishYear = (string)reader["PublishYear"],
                        CoverPrice = (decimal)reader["CoverPrice"],
                        ModifiedOn = (DateTime)reader["ModifiedOn"],
                        CheckOutStatusDescription = (string)reader["CheckOutStatusDescription"]
                    });
                }
            }



            return books;
        }


        public Book RetrieveBookDetails(int bookId)
        {
            Book book = null;

            BookCheckInCheckOutDBOperations db = BookCheckInCheckOutDBOperations.getInstance();

            IDataReader reader = db.RetrieveBooksList(bookId);            

            if (reader != null)
            {

                while (reader.Read())
                {


                    book = new Book
                    {
                        BookID = (int)reader["BookID"],
                        Title = (string)reader["Title"],
                        ISBN = (string)reader["ISBN"],
                        PublishYear = (string)reader["PublishYear"],
                        CoverPrice = (decimal)reader["CoverPrice"],
                        ModifiedOn = (DateTime)reader["ModifiedOn"],
                        CheckOutStatusDescription = (string)reader["CheckOutStatusDescription"]
                    };
                }
            }

            return book;
        }

        public List<Borrower> RetrieveBookCheckOutHistory(int bookId)
        {
            List<Borrower> borrowers = null;
            BookCheckInCheckOutDBOperations db = BookCheckInCheckOutDBOperations.getInstance();

            IDataReader reader = db.RetrieveBookCheckOutHistory(bookId);

            if (reader != null)
            {
                borrowers = new List<Borrower>();

                while (reader.Read())
                {
                    borrowers.Add(new Borrower
                    {
                        Name = (string)reader["Name"],
                        CheckOutDate = (DateTime)reader["CheckOutDate"],
                        ReturnDate = (DateTime)reader["ReturnDate"]
                    });
                }
            }

            return borrowers;
        }

        public Borrower RetrieveBookBorrowerDetails(int bookId)
        {
            Borrower borrower = null;

            BookCheckInCheckOutDBOperations db = BookCheckInCheckOutDBOperations.getInstance();

            IDataReader reader = db.RetrieveBookBorrowerDetails(bookId);

            if (reader != null)
                if (reader.Read())
                {
                    borrower = new Borrower();
                    borrower.Name = (string)reader["Name"];
                    borrower.MobileNo = (string)reader["Mobile"];
                    borrower.ReturnDate = (DateTime)reader["ReturnDate"];
                    borrower.Book = new Book()
                    {
                        ModifiedOn = (DateTime)reader["ModifiedOn"]
                    };
                }

            return borrower;
        }

        public int CheckIn(int bookId, DateTime modifiedOn)
        {
            BookCheckInCheckOutDBOperations db = BookCheckInCheckOutDBOperations.getInstance();

            return db.CheckIn(bookId, modifiedOn);
        }

        public int CheckOut(int bookID, string Name, string MobileNo, string NationalID, DateTime checkOutDate, DateTime ReturnDate, DateTime lastModifiedOn)
        {
            BookCheckInCheckOutDBOperations db = BookCheckInCheckOutDBOperations.getInstance();

            return db.CheckOut(bookID, Name, MobileNo, NationalID, checkOutDate, ReturnDate, lastModifiedOn);
        }
    }
}
