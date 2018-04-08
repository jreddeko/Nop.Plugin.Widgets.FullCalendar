nopCommerce FullCalendar Widget
===============================

Integrates FullCalendar(https://fullcalendar.io/) with nopCommerce (http://www.nopcommerce.com/images/features/responsive_devices_codeplex.jpg).

nopCommerce is the best open-source e-commerce shopping cart. nopCommerce is available for free. Today it's the best and most popular ASP.NET ecommerce software. It has been downloaded more than 1.8 million times!

nopCommerce is a fully customizable shopping cart. It's stable and highly usable. nopCommerce is an open source ecommerce solution that is **ASP.NET (MVC)** based with a MS SQL 2008 (or higher) backend database. Our easy-to-use shopping cart solution is uniquely suited for merchants that have outgrown existing systems, and may be hosted with your current web host or our hosting partners. It has everything you need to get started in selling physical and digital goods over the internet.

![nopCommerce demo](http://www.nopcommerce.com/images/features/responsive_devices_codeplex.jpg)

Directions
----------

1. Copy solution to plugins folder of nopCommerce
2. 'Copy local' property of the referenced assemblies are set to 'false'.
3. Set project output path to ..\..\Presentation\Nop.Web\Plugins\{PluginName}\ (both 'Release' and 'Debug' configurations)
4. All views (cshtml files) and web.config file should have "Build action" set to "Content" and "Copy to output directory" set to "Copy if newer"