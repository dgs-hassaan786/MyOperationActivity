using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DBAccess
{
    public class BookCheckInCheckOutDBOperations : Database
    {
        private static BookCheckInCheckOutDBOperations _instance;

        private new void Initialize()
        {
            try
            {
                base.Initialize();
            }
            catch
            {
                //do logging.

            }
        }


        public IDataReader RetrieveBooksList()
        {
            IDataReader reader = null;
            SqlCommand command = new SqlCommand();

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Constants.SP_RetrieveBooks;
                reader = base.ExecuteReader(command);
            }
            catch (Exception ex)
            {

                LogException(ex.Message,ex.ToJson(), nameof(RetrieveBookCheckOutHistory), "");

                if (!reader.IsClosed)
                    reader.Close();
                reader.Dispose();
            }
            finally
            {
                command.Dispose();
            }

            return reader;

        }

        public IDataReader RetrieveBooksList(int bookId)
        {
            IDataReader reader = null;
            SqlCommand command = new SqlCommand();

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Constants.SP_RetrieveBookDetails;

                command.Parameters.AddWithValue("@BookID", bookId);
                reader = base.ExecuteReader(command);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception("Oops! Something went wrong.");
            }
            catch (Exception ex)
            {

                var o = new { bookId };
                LogException(ex.Message,ex.ToJson(), nameof(RetrieveBooksList), o.ToJson());

                if (!reader.IsClosed)
                    reader.Close();
                reader.Dispose();
            }
            finally
            {
                command.Dispose();
            }

            return reader;

        }

        public IDataReader RetrieveBookCheckOutHistory(int bookID)
        {
            IDataReader reader = null;
            SqlCommand command = new SqlCommand();

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Constants.SP_BookBorrowingHistory;

                command.Parameters.AddWithValue("@BookID", bookID);
                reader = base.ExecuteReader(command);
            }

            catch (Exception ex)
            {

                var o = new { bookID };
                LogException(ex.Message,ex.ToJson(), nameof(RetrieveBookCheckOutHistory), o.ToJson());

                if (!reader.IsClosed)
                    reader.Close();
                reader.Dispose();
            }
            finally
            {
                command.Dispose();
            }

            return reader;

        }


        public IDataReader RetrieveBookBorrowerDetails(int bookID)
        {
            IDataReader reader = null;
            SqlCommand command = new SqlCommand();

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Constants.SP_BookBorrowerDetails;

                command.Parameters.AddWithValue("@BookID", bookID);
                reader = base.ExecuteReader(command);
            }
            catch (Exception ex)
            {

                var o = new { bookID };
                LogException(ex.Message,ex.ToJson(), nameof(RetrieveBookBorrowerDetails), o.ToJson());
                //Do logging..

                if (!reader.IsClosed)
                    reader.Close();
                reader.Dispose();
            }
            finally
            {
                command.Dispose();
            }

            return reader;

        }


        public int CheckIn(int bookID, DateTime modifiedOn)
        {
            SqlCommand command = new SqlCommand();
            int result = 0;

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Constants.SP_CheckIn;

                command.Parameters.AddWithValue("@BookID", bookID);
                command.Parameters.AddWithValue("@UTCDatetime", modifiedOn);

                //Add the output parameter to the command object
                SqlParameter outPutParameter = new SqlParameter();
                outPutParameter.ParameterName = "@oRetVal";
                outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                outPutParameter.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(outPutParameter);

                result = base.ExecuteNonQuery(command);

                var returnVal = Convert.ToInt32(outPutParameter.Value);
                return returnVal;

            }
            catch (Exception ex)
            {
                var o = new
                {
                    bookID,
                    modifiedOn
                };
                //Do logging..
                LogException(ex.Message,ex.ToJson(), nameof(CheckIn), o.ToJson());
            }
            finally
            {
                command.Dispose();
            }

            return result;

        }


        public int CheckOut(int bookID, string name, string mobileNo, string nationalID, DateTime checkOutDate, DateTime returnDate, DateTime lastModifiedOn)
        {
            SqlCommand command = new SqlCommand();
            int result = 0;

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Constants.SP_CheckOut;

                command.Parameters.AddWithValue("@BookID", bookID);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@MobileNo", mobileNo);
                command.Parameters.AddWithValue("@NationalID", nationalID);
                command.Parameters.AddWithValue("@CheckOutDate", checkOutDate);
                command.Parameters.AddWithValue("@ReturnDate", returnDate);
                command.Parameters.AddWithValue("@UTCDatetime", lastModifiedOn);

                //Add the output parameter to the command object
                SqlParameter outPutParameter = new SqlParameter();
                outPutParameter.ParameterName = "@oRetVal";
                outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                outPutParameter.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(outPutParameter);


                result = base.ExecuteNonQuery(command);

                var returnVal = Convert.ToInt32(outPutParameter.Value);
                return returnVal;
            }
            catch (Exception ex)
            {
                //Do logging..
                var o = new
                {
                    bookID,
                    name,
                    mobileNo,
                    nationalID,
                    checkOutDate,
                    returnDate,
                    lastModifiedOn
                };

                LogException(ex.Message, ex.ToJson(), nameof(CheckOut), o.ToJson());
                throw new Exception("Oops something went wrong.");
            }
            finally
            {
                command.Dispose();
            }

        }


        public int LogException(string message, string rawException, string methodName, string paramVal)
        {
            SqlCommand command = new SqlCommand();
            int result = 0;

            try
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = Constants.SP_LogException;

                command.Parameters.AddWithValue("@pMessage", message);
                command.Parameters.AddWithValue("@pRawException", rawException);
                command.Parameters.AddWithValue("@pMethodName", methodName);
                command.Parameters.AddWithValue("@pParamVal", paramVal);

                result = base.ExecuteNonQuery(command);

                return result;

            }
            catch (Exception ex)
            {

            }
            finally
            {
                command.Dispose();
            }

            return result;

        }



        //singleton implementation.
        private BookCheckInCheckOutDBOperations()
        { }

        /// <summary>
        /// Return instance for a singleton DB operations class
        /// </summary>
        /// <returns></returns>
        public static BookCheckInCheckOutDBOperations getInstance()
        {
            if (_instance == null)
            {
                _instance = new BookCheckInCheckOutDBOperations();

                _instance.Initialize();

            }

            return _instance;

        }


    }

    class Constants
    {
        public const string SP_RetrieveBooks = "usp_RetrieveBooksList";
        public const string SP_BookBorrowingHistory = "usp_getBookBorrowingHistory";
        public const string SP_BookBorrowerDetails = "usp_getBorrowerDetails";
        public const string SP_CheckIn = "usp_CheckInBook";
        public const string SP_CheckOut = "usp_CheckOutBook";
        public const string SP_RetrieveBookDetails = "usp_RetrieveBookDetails";
        public const string SP_LogException = "usp_LogException";
    }
}
