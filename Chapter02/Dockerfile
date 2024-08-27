#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/mssql/server:2022-latest

ENV SA_PASSWORD=NorthWind123456789^
ENV ACCEPT_EULA=Y

#INSTRUCTIONS
#docker build -t northwind.sqlserver .
#docker run -d --name Northwind.SqlServer -p 5553:1433 northwind.sqlserver