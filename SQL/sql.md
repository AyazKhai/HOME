# SQL Тестовые запросы

## 1. Менеджеры с номером телефона
SELECT ID, Fio 
FROM Managers 
WHERE Phone IS NOT NULL AND Phone <> '';

## 2. Количество продаж за 20 июня 2025
SELECT COUNT(*) AS SalesCount
FROM Sells
WHERE Date = '2025-06-20';

## 3. Средняя сумма продажи товара 'Фанера'
SELECT AVG(s.Sum) AS AverageSaleSum
FROM Sells s
JOIN Products p ON s.ID_Prod = p.ID
WHERE p.Name = 'Фанера';

## 4. Менеджеры и сумма продаж товара 'ОСБ'
SELECT m.Fio, SUM(s.Sum) AS TotalSalesSum
FROM Sells s
JOIN Managers m ON s.ID_Manag = m.ID
JOIN Products p ON s.ID_Prod = p.ID
WHERE p.Name = 'ОСБ'
GROUP BY m.Fio;

## 5.Продажи 22 августа 2024 (менеджер + товар)
SELECT m.Fio AS Manager, p.Name AS Product
FROM Sells s
JOIN Managers m ON s.ID_Manag = m.ID
JOIN Products p ON s.ID_Prod = p.ID
WHERE s.Date = '2024-08-22';

## 6.Товары с названием 'Фанера' и ценой от 1750
SELECT ID, Name, Cost, Volume
FROM Products
WHERE Name LIKE '%Фанера%' AND Cost >= 1750;

## 7. История продаж по месяцам и товарам
SELECT 
    EXTRACT(MONTH FROM s.Date) AS Month,
    p.Name AS ProductName,
    SUM(s.Count) AS TotalCount,
    SUM(s.Sum) AS TotalSum
FROM Sells s
JOIN Products p ON s.ID_Prod = p.ID
GROUP BY EXTRACT(MONTH FROM s.Date), p.Name
ORDER BY Month, ProductName;

## 8.Дубликаты названий товаров
SELECT 
    Name,
    COUNT(*) AS DuplicateCount
FROM Products
GROUP BY Name
HAVING COUNT(*) > 1
ORDER BY DuplicateCount DESC;
