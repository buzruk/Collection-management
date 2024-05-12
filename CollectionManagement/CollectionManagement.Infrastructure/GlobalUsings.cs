#region Usings
global using System.Net;
global using System.Text;
global using System.Text.Json;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;

global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.IdentityModel.Tokens;

global using Buzruk.GenericRepository;
global using Buzruk.GenericRepository.Async;

global using CollectionManagement.Domain.Enums;
global using CollectionManagement.Domain.Entities;
global using CollectionManagement.Domain.Entities.Admins;
global using CollectionManagement.Domain.Entities.Tags;
global using CollectionManagement.Domain.Entities.Items;
global using CollectionManagement.Domain.Entities.Users;
global using CollectionManagement.Domain.Entities.Likes;
global using CollectionManagement.Domain.Entities.Comments;
global using CollectionManagement.Domain.Entities.Collections;
global using CollectionManagement.Domain.Entities.CustomFields;

global using CollectionManagement.Infrastructure.Interfaces.Users;
global using CollectionManagement.Infrastructure.Interfaces.Tags;
global using CollectionManagement.Infrastructure.Interfaces.Common;
global using CollectionManagement.Infrastructure.Interfaces.Items;
global using CollectionManagement.Infrastructure.Interfaces.Comments;
global using CollectionManagement.Infrastructure.Interfaces.Collections;
global using CollectionManagement.Infrastructure.Interfaces.Admins;
global using CollectionManagement.Infrastructure.Interfaces.Files;
global using CollectionManagement.Infrastructure.Interfaces.Accounts;
global using CollectionManagement.Infrastructure.Interfaces.CustomFields;

global using CollectionManagement.Shared.DTOs.Accounts;
global using CollectionManagement.Shared.DTOs.Admins;
global using CollectionManagement.Shared.DTOs.Users;
global using CollectionManagement.Shared.DTOs.Collections;
global using CollectionManagement.Shared.DTOs.Comments;
global using CollectionManagement.Shared.DTOs.CustomFields;
global using CollectionManagement.Shared.DTOs.Files;
global using CollectionManagement.Shared.DTOs.Items;
global using CollectionManagement.Shared.ViewModels.AdminViewModels;
global using CollectionManagement.Shared.ViewModels.UserViewModels;
global using CollectionManagement.Shared.ViewModels.CollectionViewModels;
global using CollectionManagement.Shared.ViewModels.ItemViewModels;
global using CollectionManagement.Shared.ViewModels.CommentViewModels;
global using CollectionManagement.Shared.Exceptions;
global using CollectionManagement.Shared.Helpers;
global using CollectionManagement.Shared.Security;
global using CollectionManagement.Shared.Utils;
global using CollectionManagement.Shared.Constants;
#endregion
