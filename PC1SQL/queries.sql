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


-- 4. Mostrar el nombre de los empleados que dependan directa o indirectamente de Andrew Fuller.

