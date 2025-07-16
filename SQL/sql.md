# SQL Тестовые запросы

## 1. Менеджеры с номером телефона
SELECT ID, Fio 
FROM Managers 
WHERE Phone IS NOT NULL AND Phone <> '';

## 2. Количество продаж за 20 июня 2025
SELECT COUNT(*) AS SalesCount
FROM Sells
WHERE Date = '2025-06-20';
