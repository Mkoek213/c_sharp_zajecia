﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;

// Celem laboratorium jest zapoznanie z zastosowaniem LINQ do obsługi danych w kolekcjach. W celu realizacji laboratorium należy wczytać pliki zawierające dane zakodowane w formacie CSV a następnie dokonać na nich szeregu operacji.

// [3 punkty] odwzoruj rekordy danych z plików regions.csv, territories.csv, employee_territories.csv, employees.csv przy pomocy odpowiednich klas. Dla uproszczenia uznaj, że każde pole jest typu String. Wczytaj wszystkie dane do czterech kolekcji typu List zawierających obiekty tych klas.
// Wygodnym sposobem wczytania może być stworzenie uniwersalnej klasy wczytującej, która przy wczytywaniu rekordów będzie korzystać z metody, do której przekazywany będzie delegat tworzący obiekt odpowiedniej klasy, np.:


// class wczytywacz<T>
// {
//     public List<T> wczytajListe(String path, Func<String[], T> generuj)
//     {
//         //...
//     }
// }

// Przykładowe wywołanie:


// wczytywacz<OrderDetails> od = new wczytywacz<OrderDetails>();
// List<OrderDetails>lOrderDetailss = od.wczytajListe("c:\\projekt04\\cvs\\orders_details.csv",
//     x => new OrderDetails(x[0], x[1], x[2], x[3], x[4]));

// Gdzie OrderDetails jest konstruktorem klasy, x to tablica String ze sparsowanymi polami rekordów. Powyższy sposób to tylko sugestia - dane proszę wczytać w dowolny sposób.



class wczytywacz<T>
{
    public List<T> wczytajListe(string path, Func<string[], T> generuj)
    {
        List<T> lista = new List<T>();
        using (TextFieldParser parser = new TextFieldParser(path))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            bool firstLine = true;
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                lista.Add(generuj(fields));
            }
        }
        return lista;
    }
}


// Po wczytaniu wielokrotnie będziemy wybierać dane z list przy pomocy LINQ i wypisywać rekordy do konsoli. W niektórych przypadkach lista będzie już gotowa i nie będzie trzeba dokonywać na niej żadnej selekcji a jedynie wypisanie. W wypadku wypisywania danych pracownika można zwrócić np. jego nazwisko albo identyfikator. W wypadku pozostałych kolekcji proszę zwracać pole opisowe (nie identyfikator).

// [1 punkt] wybierz nazwiska wszystkich pracowników.

// [1 punkt] wypisz nazwiska pracowników oraz dla każdego z nich nazwę regionu i terytorium gdzie pracuje. Rezultatem kwerendy LINQ będzie "płaska" lista, więc nazwiska mogą się powtarzać (ale każdy rekord będzie unikalny).

// [1 punkt] wypisz nazwy regionów oraz nazwiska pracowników, którzy pracują w tych regionach, pracownicy mają być zagregowani po regionach, rezultatem ma być lista regionów z podlistą pracowników (odpowiednik groupjoin).

// [1 punkt] wypisz nazwy regionów oraz liczbę pracowników w tych regionach.

// [3 punkty] wczytaj do odpowiednich struktur dane z plików orders.csv oraz orders_details.csv. Następnie dla każdego pracownika wypisz liczbę dokonanych przez niego zamówień, średnią wartość zamówienia oraz maksymalną wartość zamówienia.

//wybierz nazwiska wszystkich pracowników

class Region
{
    public string RegionID { get; set; }
    public string RegionDescription { get; set; }

    public Region(string regionID, string regionDescription)
    {
        RegionID = regionID;
        RegionDescription = regionDescription;
    }
}


class Territory
{
    public string TerritoryID { get; set; }
    public string TerritoryDescription { get; set; }
    public string RegionID { get; set; }

    public Territory(string territoryID, string territoryDescription, string regionID)
    {
        TerritoryID = territoryID;
        TerritoryDescription = territoryDescription;
        RegionID = regionID;
    }
}

class EmployeeTerritory
{
    public string EmployeeID { get; set; }
    public string TerritoryID { get; set; }

    public EmployeeTerritory(string employeeId, string territoryId)
    {
        EmployeeID = employeeId;
        TerritoryID = territoryId;
    }
}

class Employee
{
    public string EmployeeID { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Title { get; set; }
    public string TitleOfCourtesy { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime HireDate { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string HomePhone { get; set; }
    public string Extension { get; set; }
    public string Photo { get; set; }
    public string Notes { get; set; }
    public string ReportsTo { get; set; }
    public string PhotoPath { get; set; }

    public Employee(string employeeId, string lastName, string firstName, string title, string titleOfCourtesy, string birthDate, string hireDate, string address, string city, string region, string postalCode, string country, string homePhone, string extension, string photo, string notes, string reportsTo, string photoPath)
    {
        EmployeeID = employeeId;
        LastName = lastName;
        FirstName = firstName;
        Title = title;
        TitleOfCourtesy = titleOfCourtesy;
        BirthDate = DateTime.ParseExact(birthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        HireDate = DateTime.ParseExact(hireDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        Address = address;
        City = city;
        Region = region;
        PostalCode = postalCode;
        Country = country;
        HomePhone = homePhone;
        Extension = extension;
        Photo = photo;
        Notes = notes;
        ReportsTo = reportsTo;
        PhotoPath = photoPath;
    }
}

class Order
{
    public string OrderID { get; set; }
    public string CustomerID { get; set; }
    public string EmployeeID { get; set; }
    public DateTime OrderDate { get; set; }
    public string RequiredDate { get; set; }
    public string ShippedDate { get; set; }
    public string ShipVia { get; set; }
    public decimal Freight { get; set; }
    public string ShipName { get; set; }
    public string ShipAddress { get; set; }
    public string ShipCity { get; set; }
    public string ShipRegion { get; set; }
    public string ShipPostalCode { get; set; }
    public string ShipCountry { get; set; }

    public Order(string orderId, string customerId, string employeeId, string orderDate, string requiredDate, string shippedDate, string shipVia, string freight, string shipName, string shipAddress, string shipCity, string shipRegion, string shipPostalCode, string shipCountry)
    {
        OrderID = orderId;
        CustomerID = customerId;
        EmployeeID = employeeId;
        OrderDate = DateTime.ParseExact(orderDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        RequiredDate = requiredDate;
        ShippedDate = shippedDate;
        ShipVia = shipVia;
        Freight = decimal.Parse(freight);
        ShipName = shipName;
        ShipAddress = shipAddress;
        ShipCity = shipCity;
        ShipRegion = shipRegion;
        ShipPostalCode = shipPostalCode;
        ShipCountry = shipCountry;
    }
}

class OrderDetails
{
    public string OrderID { get; set; }
    public string ProductID { get; set; }
    public string UnitPrice { get; set; }
    public string Quantity { get; set; }
    public string Discount { get; set; }

    public OrderDetails(string orderId, string productId, string unitPrice, string quantity, string discount)
    {
        OrderID = orderId;
        ProductID = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Discount = discount;
    }
}



class Program
{
    static void Main(string[] args)
    {
        wczytywacz<Region> regionWczytywacz = new wczytywacz<Region>();
        List<Region> regions = regionWczytywacz.wczytajListe("regions.csv", x => new Region(x[0], x[1]));

        wczytywacz<Territory> territoryWczytywacz = new wczytywacz<Territory>();
        List<Territory> territories = territoryWczytywacz.wczytajListe("territories.csv", x => new Territory(x[0], x[1], x[2]));

        wczytywacz<EmployeeTerritory> employeeTerritoryWczytywacz = new wczytywacz<EmployeeTerritory>();
        List<EmployeeTerritory> employeeTerritories = employeeTerritoryWczytywacz.wczytajListe("employee_territories.csv", x => new EmployeeTerritory(x[0], x[1]));

        wczytywacz<Employee> employeeWczytywacz = new wczytywacz<Employee>();
        List<Employee> employees = employeeWczytywacz.wczytajListe("employees.csv", x => new Employee(x[0], x[1], x[2], x[3], x[4], x[5], x[6], x[7], x[8], x[9], x[10], x[11], x[12], x[13], x[14], x[15], x[16], x[17]));

        // [1 punkt] wybierz nazwiska wszystkich pracowników.
        var employeeLastNames = employees.Select(e => e.LastName).Distinct();
        Console.WriteLine("Nazwiska wszystkich pracowników:");
        foreach (var lastName in employeeLastNames)
        {
            Console.WriteLine(lastName);
        }

        // [1 punkt] wypisz nazwiska pracowników oraz dla każdego z nich nazwę regionu i terytorium gdzie pracuje. Rezultatem kwerendy LINQ będzie "płaska" lista, więc nazwiska mogą się powtarzać (ale każdy rekord będzie unikalny).
        var employeeTerritoryDetails = from emp in employees
                                        join empTerr in employeeTerritories on emp.EmployeeID equals empTerr.EmployeeID
                                        join terr in territories on empTerr.TerritoryID equals terr.TerritoryID
                                        join reg in regions on terr.RegionID equals reg.RegionID
                                        select new
                                        {
                                            EmployeeName = emp.LastName,
                                            RegionName = reg.RegionDescription,
                                            TerritoryName = terr.TerritoryDescription
                                        };
        Console.WriteLine("\nNazwiska pracowników oraz regiony i terytoria:");
        foreach (var detail in employeeTerritoryDetails)
        {
            Console.WriteLine($"Pracownik: {detail.EmployeeName}, Region: {detail.RegionName}, Terytorium: {detail.TerritoryName}");
        }

        // [1 punkt] wypisz nazwy regionów oraz nazwiska pracowników, którzy pracują w tych regionach, pracownicy mają być zagregowani po regionach, rezultatem ma być lista regionów z podlistą pracowników (odpowiednik groupjoin).
        var regionEmployeeGroups = from reg in regions
                                    join terr in territories on reg.RegionID equals terr.RegionID
                                    join empTerr in employeeTerritories on terr.TerritoryID equals empTerr.TerritoryID
                                    join emp in employees on empTerr.EmployeeID equals emp.EmployeeID
                                    group emp by reg.RegionDescription into g
                                    select new
                                    {
                                        RegionName = g.Key,
                                        Employees = g.Select(e => e.LastName).Distinct()
                                    };

        Console.WriteLine("\nNazwy regionów oraz pracownicy w tych regionach:");
        foreach (var group in regionEmployeeGroups)
        {
            Console.WriteLine($"Region: {group.RegionName}");
            foreach (var emp in group.Employees)
            {
                Console.WriteLine($" - Pracownik: {emp}");
            }
        }

        // [1 punkt] wypisz nazwy regionów oraz liczbę pracowników w tych regionach.
        var regionEmployeeCount = from reg in regions
                                   join terr in territories on reg.RegionID equals terr.RegionID
                                   join empTerr in employeeTerritories on terr.TerritoryID equals empTerr.TerritoryID
                                   join emp in employees on empTerr.EmployeeID equals emp.EmployeeID
                                   group emp by reg.RegionDescription into g
                                   select new
                                   {
                                       RegionName = g.Key,
                                       EmployeeCount = g.Select(emp => emp.EmployeeID).Distinct().Count()
                                   };
        Console.WriteLine("\nNazwy regionów oraz liczba pracowników w tych regionach:");
        foreach (var group in regionEmployeeCount)
        {
            Console.WriteLine($"Region: {group.RegionName}, Liczba pracowników: {group.EmployeeCount}");
        }

        // [3 punkty] wczytaj do odpowiednich struktur dane z plików orders.csv oraz orders_details.csv. Następnie dla każdego pracownika wypisz liczbę dokonanych przez niego zamówień, średnią wartość zamówienia oraz maksymalną wartość zamówienia.
        wczytywacz<Order> orderWczytywacz = new wczytywacz<Order>();
        wczytywacz<OrderDetails> orderDetailsWczytywacz = new wczytywacz<OrderDetails>();
        List<Order> orders = orderWczytywacz.wczytajListe("orders.csv", x => new Order(x[0], x[1], x[2], x[3], x[4], x[5], x[6], x[7], x[8], x[9], x[10], x[11], x[12], x[13]));
        List<OrderDetails> orderDetails = orderDetailsWczytywacz.wczytajListe("orders_details.csv", x => new OrderDetails(x[0], x[1], x[2], x[3], x[4]));
        var employeeOrderStats = from emp in employees
                                  join ord in orders on emp.EmployeeID equals ord.EmployeeID into empOrders
                                  from ord in empOrders.DefaultIfEmpty()
                                  group ord by emp.LastName into g
                                  select new
                                  {
                                      EmployeeName = g.Key,
                                      OrderCount = g.Count(o => o != null),
                                      AverageOrderValue = g.Where(o => o != null).Average(o => o.Freight),
                                      MaxOrderValue = g.Where(o => o != null).Max(o => o.Freight)
                                  };
        Console.WriteLine("\nStatystyki zamówień pracowników:");
        foreach (var stat in employeeOrderStats)
        {
            Console.WriteLine($"Pracownik: {stat.EmployeeName}, Liczba zamówień: {stat.OrderCount}, Średnia wartość zamówienia: {stat.AverageOrderValue:C}, Maksymalna wartość zamówienia: {stat.MaxOrderValue:C}");
        }
    }
}

