
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            //Modified the above method from Create to Upsert to handle create and edit in 
            // single page, by checking the id is present or not
            ProductVM productVM = new()
            {
                CategoryList =  _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            if(id == null || id == 0)
            {
                //create 
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

           
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            
            if (ModelState.IsValid)
            {
                //get the WWW root path for storing the file
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                //check if the file is null
                if(file != null)
                {
                    //instead of the orginal file name create a random guid name \
                    ////with the file path and extention of the file 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    // User decides to update the image the we need to remove the old image from the folder
                    //check if the ImageUrl is null or empty
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        // get the ImageUrl from the ViewModel and Trim the starting forward slash from the string 
                        var oldImagePath = 
                            Path.Combine(wwwRootPath,productVM.Product.ImageUrl.TrimStart('\\'));
                        //delete the old image if the file exits
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    //use the file stream prepare the file for copying to the product folder
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        //copy the file to the folder
                        file.CopyTo(fileStream);
                    }
                    // assign the file name to the ImageUrl property in the Product model
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                // Section to determine if the Product should be Create or Update
                // Id present the Create the new product 
                if(productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else  // update the current product
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                //add category obj to category table
                
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);

            }
            
        }


        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id.Value == 0)
        //    {
        //        return NotFound();
        //    }
        //    // multiple ways to return id from the database
        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=> u.Id ==id);
        //    //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id ==id).FirstOrDefault();
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        //add category obj to category table
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        public IActionResult Delete(int? id)
        {
            if (id == null || id.Value == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");


        }

    }
}
