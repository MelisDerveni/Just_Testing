using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Just_Testing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Just_Testing.Controllers;

public class HomeController : Controller
{
    private MyContext _context;

    
    public HomeController(MyContext context)
    {
        _context = context;
        
    }

    
    

    public IActionResult Index()
    {
        if(HttpContext.Session.GetInt32("id") == null)
        {
        return View();
        }
        else
        {
            return RedirectToAction("Success");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }
    [HttpGet("New")]
    public IActionResult New(){
        return View();
    }
    [HttpGet("CRUDelicious")]
    public IActionResult CRUDelicious(){
        ViewBag.Mydishes = _context.Dishes.OrderBy(c=> c.Created_At);
        return View();
    }

    [HttpPost("Added")]
    

    public IActionResult Added(Dish NewDish){
        if(ModelState.IsValid)
    {
        _context.Dishes.Add(NewDish);
        _context.SaveChanges();
        return RedirectToAction("CRUDelicious");
    }
    else
    {
        // Oh no!  We need to return a ViewResponse to preserve the ModelState, and the errors it now contains!
        return View("New");
    }
        

        
    }
    [HttpGet("DisplayDish/{id}")]

    public IActionResult DisplayDish(int id){
        Dish CurrentDish = _context.Dishes.First(c=> c.DishId == id);
        return View(CurrentDish);
    }
    [HttpGet("/Delete/{id}")]
    public IActionResult Delete(int id){
        Dish DeleteDish = _context.Dishes.First(c=>c.DishId == id);
        _context.Dishes.Remove(DeleteDish);
        _context.SaveChanges();
        return RedirectToAction("CRUDelicious");
    }
    [HttpGet("/Edit/{id}")]
    public IActionResult Edit(int id){
    Dish ThisDish = _context.Dishes.First(c=> c.DishId == id);
        

        return View("Edit", ThisDish);
    }
    
    
    [HttpPost("Edit/Edited/{id}")]
    public IActionResult Edited(int id, Dish EditDish){
        if(ModelState.IsValid)
        {
        Dish CurrentDish = _context.Dishes.First(c=>c.DishId == id);
        CurrentDish.Name = EditDish.Name;
        CurrentDish.Chef= EditDish.Chef;
        CurrentDish.Tastiness = EditDish.Tastiness;
        CurrentDish.Calories = EditDish.Calories;
        CurrentDish.Description = EditDish.Description;
        CurrentDish.Updated_At = DateTime.Now;
        _context.SaveChanges();
            return RedirectToAction("Edit", id);
        }
        else
        {
            return RedirectToAction("CRUDelicious");
        }

    }

    [HttpGet("Register")]
    public IActionResult Register(){
        if(HttpContext.Session.GetInt32("id") == null){
            return View();
        }
        else
        {
            return RedirectToAction("Success");
        }

        
    }

    [HttpPost("Login")]
    public IActionResult Login(user CurrentUser)
    {
        
        if(ModelState.IsValid)
        {
            if(_context.Users.Any(u=>u.email == CurrentUser.email)){
                ModelState.AddModelError("Email", "Email already in use!");
                return View("Register");
            }
            PasswordHasher<user> Hasher = new PasswordHasher<user>();
            CurrentUser.password = Hasher.HashPassword(CurrentUser, CurrentUser.password);
            _context.Users.Add(CurrentUser);
            
            
            _context.SaveChanges();
            user FromDb = _context.Users.First(c=>c.email == CurrentUser.email );
            HttpContext.Session.SetInt32("id", FromDb.id);
            

            return RedirectToAction("Success");
            

        }
        else{
            return View("Register");
        }
        
    }
    [HttpGet("Success")]
    public IActionResult Success(){
        if(HttpContext.Session.GetInt32("id") == null){
            return View("Register");
        }
        else

        {
            
            ViewBag.Iloguari = _context.Users.First(c=>c.id ==HttpContext.Session.GetInt32("id") );
            ViewBag.Allusers = _context.Users.Where(c=>c.id != HttpContext.Session.GetInt32("id")).ToList();
            ViewBag.MyRequests = _context.Requests.Where(c=>c.ReciverId == HttpContext.Session.GetInt32("id")).Where(c=>c.Accepted == false).ToList();
            ViewBag.Myfrineds = _context.Requests.Where(c=>(c.ReciverId == HttpContext.Session.GetInt32("id"))||(c.SenderId == HttpContext.Session.GetInt32("id")) ).Where(c=>c.Accepted == true).ToList();
            ViewBag.Names = new List<user>();
            for (int i = 0; i < ViewBag.MyRequests.Count; i++)
            {
                Request CurrentReq = ViewBag.MyRequests[i];
                var shto = _context.Users.First(c=>c.id == CurrentReq.SenderId);
                ViewBag.Names.Add(shto);
            }

            return View();
        }

    }
    [HttpPost("Login2")]
    public IActionResult Login2(LoginUser UserSubmission)
    {
        if(ModelState.IsValid)
    {
        // If initial ModelState is valid, query for a user with provided email
        var userInDb = _context.Users.FirstOrDefault(u => u.email == UserSubmission.Email);
        // If no user exists with provided email
        if(userInDb == null)
        {
            // Add an error to ModelState and return to View!
            ModelState.AddModelError("Email", "Invalid Email/Password");
            return View("Index");
        }
            
        // Initialize hasher object
        var hasher = new PasswordHasher<LoginUser>();
        // verify provided password against hash stored in db
        var result = hasher.VerifyHashedPassword(UserSubmission, userInDb.password, UserSubmission.Password);
        
        // result can be compared to 0 for failure
        if(result == 0)
        {
            // handle failure (this should be similar to how "existing email" is handled)
            ModelState.AddModelError("password", "Invalid Password");
            return View("Index");
        }
        else
        {
        HttpContext.Session.SetInt32("id", userInDb.id);
        _context.SaveChanges();
        return RedirectToAction("Success");
        }
    }
    else{
    return RedirectToAction("Index");
    }
        
    }

    [HttpGet("Logout")]
    public IActionResult Logout(){
        HttpContext.Session.Clear();
        return View("Index");
    }







    [HttpGet("/DisplayCD")]
    public IActionResult DisplayCD()
    {
        List<Chef> Allchefs = _context.Chefs.Include(c=>c.CreatedDishes).ToList();
        ViewBag.AllChefs = Allchefs;
        
        return View(Allchefs);
    }
    [HttpGet("AddChef")]
    public IActionResult AddChef()
    {
        return View();
    }
    [HttpPost("AddingChef")]
    public IActionResult AddingChef(Chef NewChef)
    {
        if(ModelState.IsValid)
        {
        _context.Chefs.Add(NewChef);
        _context.SaveChanges();
        return RedirectToAction("DisplayCD");
        }
        else
        {
            return View("AddChef");
        }
    }
    [HttpGet("DisplayD")]
    public IActionResult DisplayD()
    {
        ViewBag.AllDishes = _context.Dishes2.Include(c=>c.Cook).ToList();
        
        return View();
    }
    [HttpGet ("AddDish")]
    public IActionResult AddDish()
    {
        ViewBag.AllChefs = _context.Chefs.Include(c=>c.CreatedDishes).ToList();
        return View();
    }
    [HttpPost("AddingDish")]
    public IActionResult AddingDish(Dish2 MarrNgaView, int ChefId )
    {   

        Console.WriteLine("IDDDD eshte : " + MarrNgaView.ChefId);
        MarrNgaView.ChefId = ChefId;

        if(ModelState.IsValid)
        {
            Chef user = _context.Chefs.First(e => e.ChefId == ChefId);
            user.CreatedDishes.Add(MarrNgaView);
            
            _context.Dishes2.Add(MarrNgaView);
            _context.SaveChanges();
            return RedirectToAction("DisplayD");
        }
        Console.WriteLine("Nuk eshte valid: " );
        
        ViewBag.AllChefs = _context.Chefs.Include(c=>c.CreatedDishes).ToList();
        return View("AddDish");
    }




    [HttpGet("Products")]
    public IActionResult Products()
    {
        ViewBag.AllProducts = _context.Products;
        return View();
    }

    [HttpPost("NewProduct")]
    public IActionResult NewProduct(Product MarrNgaView)
    {
        if(ModelState.IsValid)
        {
            _context.Products.Add(MarrNgaView);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }
        else
        {
            return View("Products");
        }
    }

    [HttpGet("Product/{id}")]
    public IActionResult ProductId(int id)
    {
        ViewBag.ThisProduct = _context.Products.Include(c=>c.Associations).ThenInclude(c=>c.Category).First(c=>c.ProductId == id);
        ViewBag.SelectCategories = _context.Categories.Include(c=>c.Associations).Where(c=>c.Associations.Any(c=>c.ProductId == id) == false).ToList();

        return View();
    }

    [HttpGet("Categories")]
    public IActionResult Categories()
    {
        ViewBag.AllCategories = _context.Categories;
        return View();
    }
    [HttpPost("NewCategory")]
    public IActionResult NewCategory(Category MarrNgaView)
    {
        if(ModelState.IsValid)
        {
            _context.Categories.Add(MarrNgaView);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        }
        else
        {
            return View("NewCategory");
        }
    }
    [HttpPost("/Product/AddCategory/{id}")]
    public IActionResult AddCategory(int id, SubmitCategory MarrNgaView)
    {
        Association Lidhja = new Association{
            ProductId = id,
            CategoryId = MarrNgaView.CategoryId
        };
        _context.Associations.Add(Lidhja);
        _context.SaveChanges();
        return RedirectToAction("Products");
    }

    [HttpGet("Category/{id}")]
    public IActionResult CategoryId(int id)
    {
        ViewBag.ThisCategory = _context.Categories.First(c => c.CategoryId == id);
        ViewBag.SelectProducts = _context.Products.Include(c=>c.Associations).Where(c=>c.Associations.Any(c=>c.CategoryId == id) == false).ToList();
        return View();
    }
    [HttpPost("/Category/Addproduct/{id}")]
    public IActionResult AddProduct(int id, SubmitProduct MarrNgaView)
    {
        Association Lidhja = new Association{
            ProductId = id,
            CategoryId = MarrNgaView.ProductId
        };
        _context.Associations.Add(Lidhja);
        _context.SaveChanges();
        return RedirectToAction("Categories");
    }

    [HttpGet("Request/{id}")]
    public IActionResult RequestFriend (int id)
    {
        if(HttpContext.Session.GetInt32("id") != null)
        {
        Request NewRequest = new Request{
            SenderId = _context.Users.FirstOrDefault(c=>c.id == HttpContext.Session.GetInt32("id") ).id,
            ReciverId = _context.Users.FirstOrDefault(c=>c.id == id).id
        };
        _context.Requests.Add(NewRequest);
        _context.SaveChanges();
        return RedirectToAction ("Success");
        }
        return View("Privacy");
    }
    [HttpGet("Accept/{id}")]
    public IActionResult Accept(int id)
    {
        user AcceptedUser = _context.Users.First(c=> c.id == id);
        Request AcceptedRequest = _context.Requests.First(c=> c.SenderId == AcceptedUser.id);
        AcceptedRequest.Accepted = true;
        // ViewBag.Names.Remove(AcceptedUser);
        _context.SaveChanges();
        
        return RedirectToAction("Success");
    }
    [HttpGet("Remove/{id}")]
    public IActionResult Remove(int id)
    {
        Request AcceptedRequest = _context.Requests.First(c=> c.RequestId == id);
        // user AcceptedUser = _context.Users.First(c=> c.id == id);
        // ViewBag.Names.Remove(AcceptedUser);
        return RedirectToAction("Success");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
