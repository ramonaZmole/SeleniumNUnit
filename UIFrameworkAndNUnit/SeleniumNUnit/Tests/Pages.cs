﻿using NsTestFrameworkUI.Pages;
using SeleniumNUnit.Pages;

namespace SeleniumNUnit.Tests;

public static class Pages
{
    public static Homepage Homepage = PageHelpers.InitPage(new Homepage());
    public static LoginPage LoginPage = PageHelpers.InitPage(new LoginPage());
    public static RoomsPage RoomPage = PageHelpers.InitPage(new RoomsPage());
    public static ReportPage ReportPage = PageHelpers.InitPage(new ReportPage());
    public static AdminHeaderPage AdminHeaderPage = PageHelpers.InitPage(new AdminHeaderPage());
}