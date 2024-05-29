#region Usings
global using System.Net;
global using System.Text;
global using System.Text.Json;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Net.Mail;

global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Configuration;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Routing;

global using Buzruk.GenericRepository;
global using Buzruk.GenericRepository.Async;


global using CollectionManagement.Application.Interfaces;

global using CollectionManagement.Domain.Enums;

global using CollectionManagement.Application.Interfaces.Common;

global using CollectionManagement.Shared.DTOs.Users;
global using CollectionManagement.Shared.DTOs.Collections;
global using CollectionManagement.Shared.DTOs.Comments;
global using CollectionManagement.Shared.DTOs.CustomFields;
global using CollectionManagement.Shared.DTOs.Files;
global using CollectionManagement.Shared.DTOs.Items;
global using CollectionManagement.Shared.ViewModels.UserViewModels;
global using CollectionManagement.Shared.ViewModels.CollectionViewModels;
global using CollectionManagement.Shared.ViewModels.ItemViewModels;
global using CollectionManagement.Shared.ViewModels.CommentViewModels;
global using CollectionManagement.Shared.Exceptions;
global using CollectionManagement.Shared.Helpers;
global using CollectionManagement.Shared.Utils;
global using CollectionManagement.Shared.DTOs.OneTimePassword;
#endregion
