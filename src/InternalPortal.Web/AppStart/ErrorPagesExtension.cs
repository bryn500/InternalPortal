namespace InternalPortal.Web.AppStart
{
    public static class ErrorPagesExtension
    {
        public static void UseErrorPages(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/ServerError");
                app.UseStatusCodePagesWithReExecute("/Error/Status404");
            }
        }
    }
}
