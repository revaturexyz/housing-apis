/* Helper methods for the Moq Testing of methods.
 * While not a test itself, the Test Helper assists in testing both API Coordinator methods and Data Access methods.
 * Source: Adapted from "Database and Dragons" TestHelper class.
 */

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Revature.Identity.Api;
using Revature.Identity.Api.Controllers;
using Revature.Identity.Lib.Model;

namespace Revature.Identity.Tests
{
  /// <summary>
  /// Helper methods for the Moq Testing of methods.
  /// </summary>
  public class TestHelper
  {
    public Mock<Revature.Identity.Lib.Interface.IGenericRepository> Repository { get; private set; }
    public Mock<Revature.Identity.Api.IOktaHelperFactory> OktaHelperFactory { get; private set; }

    //API Controller Instantiation
    public CoordinatorAccountController CoordinatorAccountController { get; private set; }
    public NotificationController NotificationController { get; private set; }
    public ProviderAccountController ProviderAccountController { get; private set; }
    public TenantAccountController TenantAccountController { get; private set; }

    public List<CoordinatorAccount> Coordinators { get; private set; }
    public List<Notification> Notifications { get; private set; }
    public List<ProviderAccount> Providers { get; private set; }
    public List<Status> Statuses { get; private set; }
    public List<UpdateAction> UpdateActions { get; private set; }
    public List<TenantAccount> Tenants { get; private set; }

    //for testing expiration times
    public static DateTime Now { get; set; }
    public static DateTime NowPSev { get; set; }
    public static DateTime NowPThirty { get; set; }
    public static ILogger<CoordinatorAccountController> LoggerCoord { get; set; }
    public static ILogger<NotificationController> LoggerNoti { get; set; }
    public static ILogger<ProviderAccountController> LoggerProv { get; set; }
    public static ILogger<TenantAccountController> LoggerTenant { get; set; }

    public TestHelper()
    {
      LoggerCoord = new NullLogger<CoordinatorAccountController>();
      LoggerNoti = new NullLogger<NotificationController>();
      LoggerProv = new NullLogger<ProviderAccountController>();
      LoggerTenant = new NullLogger<TenantAccountController>();

      SetUpCoordinators();
      SetUpStatuses();
      SetUpProviderAccount();
      SetUpUpdateActions();
      SetUpNotifications();
      SetUpMocks();
      SetUpTenantAccount();

      Now = DateTime.Now;
      NowPSev = Now.AddDays(7);
      NowPThirty = Now.AddDays(30);
    }

    /// <summary>
    /// Set up the example database-entries used by the "moqed" database.
    /// </summary>
    private void SetUpCoordinators()
    {
      Coordinators = new List<CoordinatorAccount>
      {
        new CoordinatorAccount
        {
          Name = "Jacob",
          Email = "jacobs.email@gmail.com",
          TrainingCenterName = "Arlington",
          TrainingCenterAddress = "604 S. West, Arlington, TX, 76010"
        },
        new CoordinatorAccount
        {
          Name = "Kimberly",
          Email = "Kimberly.email@gmail.com",
          TrainingCenterName = "Honolulu",
          TrainingCenterAddress = "555 Kaumakani St, Honolulu, HI 96825"
        },
        new CoordinatorAccount
        {
          Name = "Jimmy",
          Email = "Jimmy.email@gmail.com",
          TrainingCenterName = "NYC Midtown Center",
          TrainingCenterAddress = "348 e 66th st new york ny 10065"
        },
      };
    }


    private void SetUpProviderAccount()
    {
      Providers = new List<ProviderAccount>
      {
        new ProviderAccount
        {
          CoordinatorId = Coordinators[0].CoordinatorId,
          Name = "Billys Big Discount Dorms",
          Email = "billy@provider.org",
          Status = Statuses[0],
          AccountCreatedAt = Now,
          AccountExpiresAt = NowPSev
        },
        new ProviderAccount
        {
          CoordinatorId = Coordinators[0].CoordinatorId,
          Name = "Bobs Townhomes",
          Email = "bob@provider.org",
          Status = Statuses[1],
          AccountCreatedAt = Now,
          AccountExpiresAt = NowPSev
        },
        new ProviderAccount
        {
          CoordinatorId = Coordinators[0].CoordinatorId,
          Name = "Burgundy Hills Barracks",
          Email = "burgundy@provider.org",
          Status = Statuses[3],
          AccountCreatedAt = Now,
          AccountExpiresAt = NowPSev
        }
      };
    }


    private void SetUpTenantAccount()
    {
      Tenants = new List<TenantAccount>
      {
        new TenantAccount
        {
          Name = "Stephan",
          Email = "stephan@tenant.org"
        },
        new TenantAccount
        {
          Name = "Steffan",
          Email = "Steffan@tenant.org"
        },
        new TenantAccount
        {
          Name = "Phteven",
          Email = "Phteven@tenant.org"
        }
      };
    }


    private void SetUpUpdateActions()
    {
      UpdateActions = new List<UpdateAction>
      {
        new UpdateAction
        {
          UpdateType = "NewProvider",
          SerializedTarget = "bbbbbb"
        }
      };
    }

    private void SetUpNotifications()
    {
      Notifications = new List<Notification>
      {
        new Notification
        {
          ProviderId = Providers[0].ProviderId,
          CoordinatorId = Coordinators[0].CoordinatorId,
          Status = Statuses[0],
          UpdateAction = UpdateActions[0],
          CreatedAt = NowPSev
        },
        new Notification
        {
          ProviderId = Providers[1].ProviderId,
          CoordinatorId = Coordinators[0].CoordinatorId,
          Status = Statuses[2],
          CreatedAt = NowPSev
        },
        new Notification()
        {
          ProviderId = Providers[2].ProviderId,
          CoordinatorId = Coordinators[0].CoordinatorId,
          Status = Statuses[1],
          CreatedAt = NowPSev
        }
      };

      UpdateActions[0].NotificationId = Notifications[0].NotificationId;
    }

    private void SetUpStatuses()
    {
      Statuses = new List<Status>
      {
        new Status()
        {
          StatusText = Status.Pending
        },
        new Status()
        {
          StatusText = Status.Approved
        },
        new Status()
        {
          StatusText = Status.Rejected
        },
        new Status()
        {
          StatusText = Status.UnderReview
        }
      };
    }

    /// <summary>
    /// Instantiates the Moq instance (Mock) and seed data, specifically the controllers in the API.
    /// </summary>
    private void SetUpMocks()
    {
      Repository = new Mock<Lib.Interface.IGenericRepository>();
      OktaHelperFactory = new Mock<IOktaHelperFactory>();

      CoordinatorAccountController = new CoordinatorAccountController(Repository.Object, LoggerCoord, OktaHelperFactory.Object)
      {
        ControllerContext = new ControllerContext
        {
          HttpContext = new DefaultHttpContext()
        }
      };
      CoordinatorAccountController.ControllerContext.HttpContext.Request.Headers["Authorize"] = "Not a token.";

      ProviderAccountController = new ProviderAccountController(Repository.Object, LoggerProv, OktaHelperFactory.Object)
      {
        ControllerContext = new ControllerContext
        {
          HttpContext = new DefaultHttpContext()
        }
      };
      ProviderAccountController.ControllerContext.HttpContext.Request.Headers["Authorize"] = "Not a token.";

      NotificationController = new NotificationController(Repository.Object, LoggerNoti)
      {
        ControllerContext = new ControllerContext
        {
          HttpContext = new DefaultHttpContext()
        }
      };
      NotificationController.ControllerContext.HttpContext.Request.Headers["Authorize"] = "Not a token.";

      TenantAccountController = new TenantAccountController(Repository.Object, LoggerTenant)
      {
        ControllerContext = new ControllerContext
        {
          HttpContext = new DefaultHttpContext()
        }
      };
      NotificationController.ControllerContext.HttpContext.Request.Headers["Authorize"] = "Not a token.";
    }
  }
}
