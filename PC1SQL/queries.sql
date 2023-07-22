-- 1. Obtener la lista de los productos no descatalogados incluyendo el nombre de la categoría ordenado por nombre de producto.
SELECT Products.ProductName, Categories.CategoryName 
FROM Products 
INNER JOIN Categories on Products.CategoryID = Categories.CategoryID
WHERE Products.Discontinued = 0
ORDER BY Products.ProductName ASC;

-- 2. Mostrar el nombre de los clientes de Nancy Davolio.
SELECT DISTINCT Customers.ContactName
FROM Employees
INNER JOIN Orders ON Employees.EmployeeID = Orders.EmployeeID
INNER JOIN Customers ON Orders.CustomerID = Customers.CustomerID
WHERE Employees.FirstName = 'Nancy' AND Employees.LastName = 'Davolio'
ORDER BY Customers.ContactName ASC;

-- 3. Mostrar el total facturado por año del empleado Steven Buchanan.
SELECT YEAR(Orders.OrderDate) AS Year, SUM(od.Quantity * od.UnitPrice - (od.Quantity * od.UnitPrice * od.Discount)) AS Total 
FROM Orders
INNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID
INNER JOIN [Order Details] od ON Orders.OrderID = od.OrderID
WHERE Employees.FirstName = 'Steven' AND Employees.LastName = 'Buchanan'
GROUP BY YEAR(Orders.OrderDate)
ORDER BY Year ASC;

-- 4. Mostrar el nombre de los empleados que dependan directa o indirectamente de Andrew Fuller.
WITH cte_Employees AS (
    SELECT EmployeeID, FirstName, LastName, ReportsTo
    FROM Employees
    WHERE FirstName = 'Andrew' AND LastName = 'Fuller'
    
    UNION ALL
    
    SELECT e.EmployeeID, e.FirstName, e.LastName, e.ReportsTo
    FROM Employees e
    INNER JOIN cte_Employees re ON e.ReportsTo = re.EmployeeID
)
SELECT FirstName, LastName
FROM cte_Employees
WHERE FirstName != 'Andrew' AND LastName != 'Fuller';
