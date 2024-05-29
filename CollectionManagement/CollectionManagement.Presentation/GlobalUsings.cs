#region Usings
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;

global using System.Net;
global using System.Text.Json;
global using System.Text;
global using System.Diagnostics;

global using Buzruk.GenericRepository.Async;
global using Serilog;
global using Serilog.Events;
global using Asp.Versioning;



global using CollectionManagement.Shared.Helpers;
global using CollectionManagement.Shared.DTOs.OneTimePassword;
global using CollectionManagement.Shared.Utils;
global using CollectionManagement.Shared.Exceptions;
global using CollectionManagement.Shared.DTOs.Collections;
global using CollectionManagement.Shared.DTOs.Comments;
global using CollectionManagement.Shared.ViewModels;
global using CollectionManagement.Shared.DTOs.Items;
global using CollectionManagement.Shared.DTOs.CustomFields;
global using CollectionManagement.Domain.Entities;
global using CollectionManagement.Shared.DTOs.Users;
global using CollectionManagement.Shared.DTOs;
global using CollectionManagement.Shared.Constants;

global using CollectionManagement.Application.Services;
global using CollectionManagement.Application.Interfaces;
global using CollectionManagement.Application.Interfaces.Common;
global using CollectionManagement.Application.Services.Common;

global using CollectionManagement.Presentation.Configurations;
global using CollectionManagement.Presentation.Configurations.LayerConfigurations;
global using CollectionManagement.Presentation.Middlewares;

global using CollectionManagement.Infrastructure.DbContexts;
#endregion
