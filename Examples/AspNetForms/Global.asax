<%@ Application Language="C#" %>
<%@ Import Namespace="Chessar" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Hooks.PatchLongPaths();
    }

    void Application_End(object sender, EventArgs e)
    {
        // Code that runs on application shutdown
        Hooks.RemoveLongPathsPatch();
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
    }

</script>
