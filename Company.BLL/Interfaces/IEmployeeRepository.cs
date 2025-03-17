using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Models;

namespace Company.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Employee Get(int? id);

        //IEnumerable<Employee> GetAll();

        //Employee? Get(int id);

        List<Employee> GetByName(string name);

        //int Add(Employee model);
        //int Update(Employee model);
        //int Delete(Employee model);
    }
}
