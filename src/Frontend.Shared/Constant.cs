namespace Frontend.Shared
{
    public static class Constant
    {

        public static class AdminAudit
        {
            public const string Base = "admin/audit";
        }

        public static class Administration
        {
            public const string AdminRole = "Admin";
        }

        public static class AdminSettings
        {
            public const string Base = "admin/settings";
        }

        public static class AdminUsers
        {
            public const string Base = "admin/users";
            public const string Roles = "admin/users/roles";
        }

        public static class ApiCallType
        {
            public const string Delete = "delete";
            public const string Get = "get";
            public const string Post = "post";
            public const string Update = "update";
        }

        public static class ApiClient
        {
            public const string PrivateName = "Blazor-Client-Private";
            public const string PublicName = "Blazor-Client-Public";
        }

        public static class Authentication
        {
            public const string ChangePassword = "authentication/change-password";
            public const string ConfirmEmail = "authentication/confirm-email";
            public const string EndDemo = "demo/session";
            public const string Login = "authentication/login";
            public const string Logout = "authentication/logout";
            public const string Register = "authentication/create";
            public const string ReviveToke = "authentication/refresh-token";
            public const string StartDemo = "demo/sessions";
            public const string Type = "Bearer";
            public const string UpdateProfile = "authentication/update-profile";
        }

        public static class Document
        {
            public const string Add = "document/add";
            public const string Delete = "document/delete";
            public const string Get = "document/single";
            public const string GetAll = "document/all";
            public const string GetAllForAdmin = "document/all/admin";
            public const string GetDocumentByDocumenttype = "document/documents-by-type";
            public const string Update = "document/update";
        }

        public static class DocumentType
        {
            public const string Add = "document-type/add";
            public const string Delete = "document-type/delete";
            public const string Get = "document-type/single";
            public const string GetAll = "document-type/all";
            public const string GetAllForAdmin = "document-type/all/admin";
            public const string Update = "document-type/update";
        }

        public static class File
        {
            public const string Upload = "upload/image";
        }

        public static class Seo
        {
            public const string AdminSettings = "admin/seo/settings";
            public const string Categories = "admin/categories";
            public const string Products = "admin/products";
            public const string Redirects = "admin/seo/redirects";
            public const string Settings = "seo/settings";
        }
        public static class TokenStorage
        {
            public const string Key = "token";
        }
    }
}
