using System.Collections.Generic;
using System.Data.SqlClient;
using BookStore.model;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private string connectionString = ConnectionString.connectionString;

        // GET: api/Books
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            string selectString = "select * from books2";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(selectString, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Book> result = new List<Book>();
                        while (reader.Read())
                        {
                            Book book = ReadBook(reader);
                            result.Add(book);
                        }
                        return result;
                    }
                }
            }
        }

        private Book ReadBook(SqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string title = reader.GetString(1);
            string author = reader.GetString(2);
            string publisher = reader.IsDBNull(3) ? null : reader.GetString(3);
            decimal price = reader.GetDecimal(4);
            Book book = new Book
            {
                Id = id,
                Title = title,
                Author = author,
                Publisher = publisher,
                Price = price
            };
            return book;
        }

        // GET: api/Books/5
        [HttpGet]
        [Route("{id}")]
        public Book Get(int id)
        {
            string selectString = "select * from books2 where id = @id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(selectString, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return ReadBook(reader);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        // POST: api/Books
        [HttpPost]
        public int Post([FromBody] Book value)
        {
            string insertString = "insert into books2 (title, author, publisher, price) values (@title, @author, @publisher, @price);";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(insertString, conn))
                {
                    command.Parameters.AddWithValue("@title", value.Title);
                    command.Parameters.AddWithValue("@author", value.Author);
                    command.Parameters.AddWithValue("@publisher", value.Publisher);
                    command.Parameters.AddWithValue("@price", value.Price);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] Book value)
        {
            const string updateString =
                "update books2 set title=@title, author=@author, publisher=@publisher, price=@price where id=@id;";
            using (SqlConnection databaseConnection = new SqlConnection(connectionString))
            {
                databaseConnection.Open();
                using (SqlCommand updateCommand = new SqlCommand(updateString, databaseConnection))
                {
                    updateCommand.Parameters.AddWithValue("@title", value.Title);
                    updateCommand.Parameters.AddWithValue("@author", value.Author);
                    updateCommand.Parameters.AddWithValue("@publisher", value.Publisher);
                    updateCommand.Parameters.AddWithValue("@price", value.Price);
                    updateCommand.Parameters.AddWithValue("@id", id);
                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            string deleteString = "delete from books2 where id = @id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(deleteString, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }
    }
}
