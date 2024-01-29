using System;
using System.Linq;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Models;
using System.Reflection;
using NHibernate;
using System.Diagnostics;


static void PomiarCzasu(Action operation)
{
    var zegar = Stopwatch.StartNew();
    operation();
    zegar.Stop();
    Console.WriteLine("Czas wynosi: {0}ms", zegar.ElapsedMilliseconds);
}


var zegar = Stopwatch.StartNew();
var connString = "Data Source=DESKTOP-1MAQ4AD\\TEW_SQLEXPRESS;Initial Catalog=NorthwindNHibernateTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
var config = new Configuration();
zegar.Stop();
Console.WriteLine($"Czas wlaczenia wynosi: {zegar.ElapsedMilliseconds}ms");

config.DataBaseIntegration(x =>
{
    x.ConnectionString = connString;
    x.Dialect<MsSql2012Dialect>();
    x.Driver<SqlClientDriver>();
});

config.AddAssembly(Assembly.GetExecutingAssembly());
var sessionFactory = config.BuildSessionFactory();

using var session = sessionFactory.OpenSession();

//create
var addEmployeeOperation = () =>
{
    var employee = session.Save(new Employee()
    {
        FirstName = "Karol",
        LastName = "Kowalski"
    });
};
Console.WriteLine("addEmployeeOperation");
PomiarCzasu(addEmployeeOperation);

//read
var employeeIdToRead = 24;
var readEmployeeOperation = () =>
{
    var employee = session.Get<Employee>(employeeIdToRead);
    if (employee == null)
    {
        Console.WriteLine("Nie istnieje");
    }
    else
    {
        Console.WriteLine($"Imie: {employee.FirstName} Nazwisko: {employee.LastName}");
    }
};
PomiarCzasu(readEmployeeOperation);

//update
int employeeIdToUpdate = 1;

var UpdateEmployeeOperation = () =>
{
    var employeeToEdit = session.Get<Employee>(employeeIdToUpdate);
    if (employeeToEdit != null)
    {
        employeeToEdit.Country = "Polska";
        using var transaction = session.BeginTransaction();

        session.Update(employeeToEdit);
        transaction.Commit();
        Console.WriteLine($"Dane pracownika o ID {employeeIdToUpdate} zostały zaktualizowane.");
    }
    else
    {
        Console.WriteLine($"Pracownik o ID {employeeIdToUpdate} nie istnieje.");
    }
};
PomiarCzasu(UpdateEmployeeOperation);

//delete
int employeeIdToDelete = 30;

var deleteEmployeeOperation = () =>
{
    var employeeToDelete = session.Get<Employee>(employeeIdToDelete);

    if (employeeToDelete != null)
    {
        using var transaction = session.BeginTransaction();
        session.Delete(employeeToDelete);
        transaction.Commit();
        Console.WriteLine($"Pracownik o ID {employeeIdToDelete} został usunięty.");
    }
    else
    {
        Console.WriteLine($"Pracownik o ID {employeeIdToDelete} nie istnieje.");
    }
};
PomiarCzasu(deleteEmployeeOperation);

/*
PomiarCzasu(addEmployeeOperation);
PomiarCzasu(addEmployeeOperation);
PomiarCzasu(addEmployeeOperation);
Console.WriteLine("\n\n\n");

var readEmployeeOperationId2 = () =>
{
    var employee = session.Get<Employee>(2);
};

Console.WriteLine("readEmployeeOperation");
PomiarCzasu(readEmployeeOperation);
PomiarCzasu(readEmployeeOperation);
Console.WriteLine("\n\n\n");

Console.WriteLine("readEmployeeOperationId2");
PomiarCzasu(readEmployeeOperationId2);
Console.WriteLine("\n\n\n");


var getAllEmployees = () =>
{
    var employees = session.QueryOver<Employee>().List<Employee>().ToList();
};
Console.WriteLine("getAllEmployees");
PomiarCzasu(getAllEmployees);
PomiarCzasu(getAllEmployees);
Console.WriteLine("\n\n\n");*/



/*var emp = new Employee { FirstName = "", LastName = "" };
session.Save(emp);

var employees = session.QueryOver<Employee>().List<Employee>();
foreach (var employee in employees)
{
    Console.WriteLine($"{employee.EmployeeID}: {employee.FirstName} {employee.LastName}");
}
*/