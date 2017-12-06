@echo off
start /d "../../src/Bank.Cards.Admin.Api/" dotnet run 
start /d "../../src/Bank.Cards.Transactional.Api/" dotnet run 
start /d "../../src/Bank.Cards.IdentityServer/" dotnet run 