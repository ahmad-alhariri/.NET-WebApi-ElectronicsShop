namespace ElectronicsShop.Api.MetaData;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;

    public static class Categories
    {
        private const string Resource = "/categories";
        
        public const string GetAll = Base + Resource;
        public const string GetById = Base + Resource + "/{id}";

        public const string Create = Base + Resource;
        public const string Update = Base + Resource + "/{id}";
        public const string Delete = Base + Resource + "/{id}";
        
        public const string ProductsByCategory = Base + Resource + "/{id}/products";
    }

    public static class Brands
    {
        private const string Resource = "/brands";

        public const string GetAll = Base + Resource;
        public const string GetById = Base + Resource + "/{id}";

        public const string Create = Base + Resource;
        public const string Update = Base + Resource + "/{id}";
        public const string Delete = Base + Resource + "/{id}";
        
        public const string ProductsByBrand = Base + Resource + "/{id}/products";
    }

    public static class Products
    {
        private const string Resource = "/products";
        
        public const string GetAll = Base + Resource;
        public const string GetById = Base + Resource + "/{id}";
        public const string Search = Base + Resource + "/search";
        public const string LowStock = Base + Resource + "/low-stock";
        public const string FeaturedProducts = Base + Resource + "/featured";
        public const string NewProducts = Base + Resource + "/new";
        public const string Statistics = Base + Resource + "/statistics";
        
        public const string Create = Base + Resource;
        public const string Update = Base + Resource + "/{id}";
        public const string Delete = Base + Resource + "/{id}";
        
        public const string AddImages = Base + Resource + "/{id}/images";
        public const string RemoveImage = Base + Resource + "/{id}/images/{imageId}";
        public const string RemoveImages = Base + Resource + "/{id}/images";
        public const string SetPrimaryImage = Base + Resource + "/{id}/images/{imageId}/set-primary";
        
        public const string UpdateStock = Base + Resource + "/{id}/update-stock";
        public const string UpdatePrice = Base + Resource + "/{id}/update-price";
        
        public const string Featured = Base + Resource + "/{id}/featured";
        
        public const string AddSpecifications = Base + Resource + "/{id}/specifications";
        public const string RemoveSpecification = Base + Resource + "/{id}/specifications/{key}";
        public const string ClearSpecifications = Base + Resource + "/{id}/specifications";

    }

    public static class Users
    {
        private const string Resource = "/users";
        public const string GetAll = Base + Resource;
        public const string GetById = Base + Resource + "/{email}";
        public const string Create = Base + Resource;
        public const string Update = Base + Resource;
        public const string Delete = Base + Resource + "/{id}";
        public const string ChangePassword = Base + Resource + "/{id}/change-password";
        public const string Paginated = Base + Resource + "/paginated";
    }

    public static class Auth
    {
        private const string Resource = "/auth";
        public const string Register = Base + Resource + "/register";
        public const string SignIn = Base + Resource + "/signin";
        public const string RefreshToken = Base + Resource + "/refresh-token";
        public const string ValidateToken = Base + Resource + "/validate-token";
        public const string ConfirmEmail = Base + Resource + "/confirm-email";
        public const string ResendConfirmEmail = Base + Resource + "/resend-confirm-email";
        public const string SendResetPasswordCode = Base + Resource + "/reset-password/code";
        public const string ConfirmResetPassword = Base + Resource + "/reset-password/confirm";
        public const string ResetPassword = Base + Resource + "/reset-password";
    }

    // public static class Roles
    // {
    //     private const string Resource = "/roles";
    //     public const string GetAll = Base + Resource;
    //     public const string GetById = Base + Resource + "/{id}";
    //     public const string Create = Base + Resource;
    //     public const string Update = Base + Resource + "/{id}";
    //     public const string Delete = Base + Resource + "/{id}";
    //     public const string AssignUserRoles = Base + Resource + "/{id}/users";
    // }

    // public static class Claims
    // {
    //     private const string Resource = "/claims";
    //     public const string ManageUserClaims = Base + Resource + "/{userId}/manage";
    //     public const string UpdateUserClaims = Base + Resource + "/{userId}/update";
    // }

    public static class Emails
    {
        private const string Resource = "/emails";
        public const string Send = Base + Resource + "/send";
    }

    public static class Cart
    {
        private const string Resource = "/cart";
        public const string GetCart = Base + Resource;
        public const string AddItem = Base + Resource + "/add-item";
        public const string UpdateItem = Base + Resource + "/update-item";
        public const string RemoveItem = Base + Resource + "/remove-item";
        public const string ClearCart = Base + Resource + "/clear";
    }

    public static class Wishlists
    {
        private const string Resource = "/wishlists";
        public const string GetWishlist = Base + Resource;
        public const string AddItem = Base + Resource + "/add-item";
        public const string RemoveItem = Base + Resource + "/remove-item";
    }

    public static class Addresses
    {
        private const string Resource = "/addresses";
        public const string GetAll = Base + Resource;
        public const string Create = Base + Resource;
        public const string Update = Base + Resource + "/{id}";
        public const string Delete = Base + Resource + "/{id}";
        public const string SetDefault = Base + Resource + "/set-default/{id}";
    }

    public static class Promotions
    {
        private const string Resource = "/promotions";
        public const string GetAll = Base + Resource;
        public const string GetById = Base + Resource + "/{id}";
        public const string Create = Base + Resource;
        public const string Update = Base + Resource + "/{id}";
        public const string Delete = Base + Resource + "/{id}";
        public const string ValidateCode = Base + Resource + "/validate-code";
    }

    public static class Discounts
    {
        private const string Resource = "/discounts";
        public const string GetAll = Base + Resource;
        public const string GetById = Base + Resource + "/{id}";
        public const string Create = Base + Resource;
        public const string Update = Base + Resource + "/{id}";
        public const string Delete = Base + Resource + "/{id}";
        public const string ValidateCode = Base + Resource + "/validate-code";
    }


    public static class Orders
    {
        private const string Resource = "/orders";
        public const string GetAll = Base + Resource;
        public const string Create = Base + Resource;
        public const string Update = Base + Resource + "/{id}";
        public const string Delete = Base + Resource + "/{id}";
    }

    public static class Payments
    {
        private const string Resource = "/payments";
        public const string CreatePaymentIntent = Base + Resource + "/create-payment-intent";
        public const string ConfirmPayment = Base + Resource + "/confirm-payment";
        public const string GetByPaymentId = Base + Resource + "/{id}";
        public const string Webhook = Base + "/webhooks/stripe";
    }
}