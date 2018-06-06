using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone6.Models;

namespace Capstone6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Error = "";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        //the user in parenthesis my table name-the newUser is a variable
        public ActionResult RegisterNewUser(user newUser)
        {
            //adding user to the database
            TaskListEntities ORM = new TaskListEntities();
            //call users (the name of the db I want to add to)
            //--tell it to add info the newUser variable
            ORM.users.Add(newUser);
            //save the changes to the db (users)
            ORM.SaveChanges();
            //user gets directed to the home page after entering new user
            return View("Index");
        }

        //make sure db has the person and then make sure its a match
        public ActionResult SignIn(string UserName, string Password)
        {
            TaskListEntities ORM = new TaskListEntities();

            user currentUser = ORM.users.Find(UserName);
            if (currentUser == null)
            {
                ViewBag.Error = "Username does not exist.";
                return View("Index");
            }
            else if (currentUser.Password != Password)
            {
                ViewBag.Error = "Incorrect password";
                return View("Index");
            }
            ViewBag.Message = $"Welcome {UserName}!";

            ViewBag.UserTask = ORM.Tasks.ToList();
            return View("Welcome");
        }

        public ActionResult AddTask()
        {

            return View ();
        }

        public ActionResult SaveNewTask(Task NewTask)
        {
            TaskListEntities ORM = new TaskListEntities();
            if (ORM.Tasks.ToList().Count() == 0)
            {
                NewTask.TaskNumber = "1";
            }
            else
            {
                NewTask.TaskNumber = (int.Parse(ORM.Tasks.ToList().OrderBy(x => int.Parse(x.TaskNumber)).Last().TaskNumber) + 1).ToString();
                
            }

            NewTask.user = ORM.users.Find(NewTask.UserName);

            ORM.Tasks.Add(NewTask);
            ORM.SaveChanges();
            return RedirectToAction("Welcome");
        }
        

        public ActionResult EditTask(string TaskNumber)
        {
            TaskListEntities ORM = new TaskListEntities();
            ViewBag.Task = ORM.Tasks.Find(TaskNumber);
            return View();
        }

        public ActionResult SaveUpdatedTask(Task updatedTask)
        {
            TaskListEntities ORM = new TaskListEntities();

            Task toBeUpdated = ORM.Tasks.Find(updatedTask.TaskNumber);

            if (toBeUpdated!= null && ModelState.IsValid)
            {
                toBeUpdated.UserName = updatedTask.UserName;
                toBeUpdated.Description = updatedTask.Description;
                toBeUpdated.DueDate = updatedTask.DueDate;
                toBeUpdated.Status = updatedTask.Status;

                ORM.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            ORM.SaveChanges();
            return RedirectToAction("Welcome");

            }
            else
            {
                ViewBag.ErrorMessage = "Item not edited";
                return View("Error");
            }
        }

        public ActionResult DeleteTask(string TaskNumber)
        {
            TaskListEntities ORM = new TaskListEntities();
            Task deletedtask = ORM.Tasks.Find(TaskNumber);
            try
            {
                ORM.Tasks.Remove(deletedtask);
                ORM.SaveChanges();
            }
            catch
            {
                ViewBag.Message = "Error";
                return View("Error");
            }

            ViewBag.UserTask = ORM.Tasks.ToList();
            return View("Welcome");
        }

         
        public ActionResult Welcome()
        {
            TaskListEntities ORM = new TaskListEntities();
            ViewBag.UserTask = ORM.Tasks.ToList();
            return View();
        }

        


    }
        
        

        
            

        
    
}