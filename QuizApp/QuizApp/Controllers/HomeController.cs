using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizApp.Controllers
{
    public class HomeController : Controller
    {
        QuizAppEntities db = new QuizAppEntities();

        [HttpGet]
        public ActionResult tlogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult tlogin(admin admin)
        {
            admin a = db.admins.Where(x => x.adminName == admin.adminName && x.adminPassword == admin.adminPassword).SingleOrDefault();
            if( a != null)
            {
                Session["adminId"] = a.adminId;
               
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.msg = "Invalid User Name or Password";
            }
            return View();
        }
        public ActionResult slogin()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add_Course()
        {
            //Session["adminId"] = 1;//we will remove it soon
            int adminId = Convert.ToInt32(Session["adminId"].ToString());
            System.Diagnostics.Debug.WriteLine("this is the session admin id :" +Session["adminId"]);
            
            List<course> courseList = db.courses.Where(x=>x.courseAdminId == adminId ).OrderByDescending(x => x.courseId).ToList();
            ViewData["list"] = courseList;
            return View();
        }

        [HttpPost]
        public ActionResult Add_Course(course c)
        {
            int adminId = Convert.ToInt32(Session["adminId"].ToString());
            List<course> courseList = db.courses.Where(x => x.courseAdminId == adminId).OrderByDescending(x => x.courseId).ToList();
            ViewData["list"] = courseList;
            course course = new course();
            course.courseName = c.courseName;
            course.courseAdminId = adminId;


            db.courses.Add(course);
            db.SaveChanges();
            return RedirectToAction("Add_Course");
        }

        [HttpGet]
        public ActionResult AddQuestions()
        {
            int adminId = Convert.ToInt32(Session["adminId"].ToString());
            List<course> courseList = db.courses.Where(x => x.courseAdminId == adminId).ToList();
            ViewBag.list = new SelectList(courseList, "courseId", "CourseName");

            return View();
        }

        [HttpPost]
        public ActionResult AddQuestions(question q)
        {
            int adminId = Convert.ToInt32(Session["adminId"]);
            System.Diagnostics.Debug.WriteLine("this is the session admin id : (from addQuestion post)" + Session["adminId"]);
            List<course> courseList = db.courses.Where(x => x.courseAdminId == adminId).ToList();
            ViewBag.list = new SelectList(courseList, "courseId", "CourseName");

            question qa = new question();
            qa.questionText = q.questionText;
            qa.optionA = q.optionA;
            qa.optionB = q.optionB;
            qa.optionC = q.optionC;
            qa.optionD = q.optionD;
            qa.correctAnswer = q.correctAnswer;
            qa.questionCourseId = q.questionCourseId;

            db.questions.Add(qa);
            db.SaveChanges();
            ViewBag.ms = "Question Successfully Added";
            return View();
        }

        public ActionResult Index()
        {
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
    }
}