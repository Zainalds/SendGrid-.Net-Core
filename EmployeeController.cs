using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            string username = User.Identity.Name;

            var list = (from x in DapperORM.ReturnList<Authorization.Models.Employee>("EmployeeViewAll")
                        where x.UserId == username
                        select x).ToList();
            return View(list);
        }



        [HttpGet]
        public IActionResult AddorEdit(int id = 0)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                DynamicParameters parm = new DynamicParameters();
                parm.Add("@EmployeeID", id);
                return View(DapperORM.ReturnList<Authorization.Models.Employee>("EMployeeViewByID", parm).FirstOrDefault<Authorization.Models.Employee>());
            }
        }

        [HttpPost]
        public IActionResult AddorEdit(Authorization.Models.Employee emp)
        {
            string username = User.Identity.Name;
            DynamicParameters param = new DynamicParameters();
            param.Add("@EmployeeID", emp.EmployeeID);
            param.Add("@EmployeeName", emp.Employee_Name);
            param.Add("@EmployeePosition", emp.Employee_Posititon);
            param.Add("@EmployeeAge", emp.Employee_Age);
            param.Add("@EmployeeSalary", emp.Employee_Salary);
            param.Add("@UserId", username);
            DapperORM.ExecuteWithoutReturn("EMployeeAddOrEdit", param);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            DynamicParameters parm = new DynamicParameters();
            parm.Add("@EmployeeID", id);
            DapperORM.ExecuteWithoutReturn("EmployeeDeleteByID", parm);
            return RedirectToAction("Index");
        }
    }
}
