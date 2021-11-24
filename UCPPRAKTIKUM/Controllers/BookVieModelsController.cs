using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UCPPRAKTIKUM.Data;
using UCPPRAKTIKUM.Models;

namespace UCPPRAKTIKUM.Controllers
{
    public class BookVieModelsController : Controller
    {
        private readonly IConfiguration _configuration;

        public BookVieModelsController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: BookVieModels
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("BookViewAll", sqlConnection);
                sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlda.Fill(dtbl);

            }

            return View(dtbl);

        }

        // GET: BookVieModels/AddOrEdit/ 
        public IActionResult AddOrEdit(int? id)
        {
            BookVieModel bookVieModel = new BookVieModel();
            if (id > 0)
                bookVieModel = FETCHBookByID(id);
            return View(bookVieModel);
        }

        // POST: BookVieModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("BookID,Title,Author,Price")] BookVieModel bookVieModel)
        {
            if (ModelState.IsValid)
            {
               using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlcmd = new SqlCommand("BookAddOrEdit", sqlConnection);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@BookID", bookVieModel.BookID);
                    sqlcmd.Parameters.AddWithValue("@Author", bookVieModel.Author);
                    sqlcmd.Parameters.AddWithValue("@Title", bookVieModel.Title);
                    sqlcmd.Parameters.AddWithValue("@Price", bookVieModel.Price);
                    sqlcmd.ExecuteNonQuery();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(bookVieModel);
        }

        // GET: BookVieModels/Delete/5
        public IActionResult Delete(int? id)
        {

            BookVieModel bookVieModel = FETCHBookByID(id);
            return View();
        }

        // POST: BookVieModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletedFirmed(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlcmd = new SqlCommand("BookDeleteByID", sqlConnection);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@BookID", id);

                sqlcmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public BookVieModel FETCHBookByID(int? id)
        {
            BookVieModel bookVieModel = new BookVieModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlda = new SqlDataAdapter("BookViewByID", sqlConnection);
                sqlda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlda.SelectCommand.Parameters.AddWithValue("BookID", id);
                sqlda.Fill(dtbl);
                if (dtbl.Rows.Count ==1)
                {
                    bookVieModel.BookID =Convert.ToInt32(dtbl.Rows[0]["BookID"].ToString());
                    bookVieModel.Title = dtbl.Rows[0]["Title"].ToString();
                    bookVieModel.Author = dtbl.Rows[0]["Author"].ToString();
                    bookVieModel.Price = Convert.ToInt32(dtbl.Rows[0]["Price"].ToString());

                }
                return bookVieModel;

            }
        }
    }

}
