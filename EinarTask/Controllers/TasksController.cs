using System;
using System.Security.Claims;
using System.Text.Json;
using EinarTask.Data;
using EinarTask.Models;
using EinarTask.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EinarTask.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment environment;


        public TasksController(ApplicationDbContext context, UserManager<User> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            this.environment = environment;
        }

        // TasksController.cs
       

        // Diğer action'ları da ViewModel ile uyumlu hale getirin

        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            var userId = int.Parse(userIdStr);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userInfo = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.FirstName, u.LastName })
                .FirstOrDefaultAsync();

            ViewBag.UserFirstandLastName = $"{userInfo?.FirstName} {userInfo?.LastName}";
           
            await EnsureDefaultTaskTypes(userId);

            var viewModel = new TaskBoardViewModel  
            {
                TaskTypes = await _context.TaskTypes
                    .Where(t => t.UserId == userId)
                    .Include(t => t.Tasks) // Taskı include olarak içinde dahil ediyorum
                    .OrderBy(t => t.Order)
                    .ToListAsync(), // Asenron ile listeliyorum noorderby

                AllTasks = await _context.Tasks
                    .Where(t => t.UserId == userId)
                    .ToListAsync(),

                NewTask = new Models.Task { UserId = userId },
                NewTaskType = new TaskType { UserId = userId }
            };
            ViewBag.UserImage = user.UserImage;
            return View(viewModel);
        }

        private async System.Threading.Tasks.Task EnsureDefaultTaskTypes(int userId)
        {
            if (!await _context.TaskTypes.AnyAsync(t => t.UserId == userId))
            {
                var defaultTaskTypes = new List<TaskType>
                {
                    new TaskType { Name = "ToDo", Description = "", Color = "#ff6347", Order = 1, UserId = userId, CreatedDate = DateTime.Now },
                    new TaskType { Name = "Doing", Description = "", Color = "#ffa500", Order = 2, UserId = userId, CreatedDate = DateTime.Now },
                    new TaskType { Name = "Done", Description = "", Color = "#28a745", Order = 3, UserId = userId, CreatedDate = DateTime.Now }
                };
                _context.TaskTypes.AddRange(defaultTaskTypes);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Varsayılan TaskType'lar oluşturulurken bir hata oluştu.", ex);
                }
            }
        }

        [HttpGet]
        public IActionResult CreateTask(int? userId)
        {
            if (userId == null || userId == 0)
            {
                userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (userId == null)
                {
                    return new StatusCodeResult(400); // Bad Request
                }
            }
            Console.WriteLine($"CreateTask GET - UserId: {userId}");
            ViewBag.UserId = userId;
            ViewBag.TaskTypes = _context.TaskTypes.Where(t => t.UserId == userId).ToList();
            return PartialView("_CreateTaskModal");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(Models.Task task)
        {
            Console.WriteLine($"CreateTask POST - Received Task: UserId={task.UserId}, TaskTypeId={task.TaskTypeId}, Title={task.Title}");
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Manuel olarak UserId ve TaskTypeId set et
            task.UserId = userId;
            var availableTaskTypes = await _context.TaskTypes
                .Where(t => t.UserId == userId)
                .ToListAsync();
            if (availableTaskTypes.Any())
            {
                Random rand = new Random();
                task.TaskTypeId = availableTaskTypes[rand.Next(availableTaskTypes.Count)].Id;
                Console.WriteLine($"Assigned TaskTypeId: {task.TaskTypeId}");
            }
            else
            {
                return Json(new { success = false, errors = new[] { "Hiç TaskType bulunamadı, lütfen bir TaskType oluşturun." } });
            }

            // ModelState'i tamamen bypass et, sadece gerekli alanları al
            task.CreatedDate = DateTime.Now;
            task.Description = task.Description ?? "";
            task.DueDate = task.DueDate;
            task.Priority = task.Priority > 0 ? task.Priority : 1;
            task.IsCompleted = false;

            _context.Tasks.Add(task);
            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                return Json(new { success = false, errors = new[] { "Veritabanı hatası: " + ex.Message } });
            }
        }

        [HttpGet]
        public IActionResult CreateOrUpdateTaskType(int? id, int? userId)
        {
            if (userId == null || userId == 0)
            {
                userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (userId == null)
                {
                    return new StatusCodeResult(400); // Bad Request
                }
            }
            Console.WriteLine($"CreateOrUpdateTaskType GET - Id: {id}, UserId: {userId}");
            if (id.HasValue && id > 0)
            {
                var taskType = _context.TaskTypes.FirstOrDefault(t => t.Id == id && t.UserId == userId);
                if (taskType == null)
                {
                    return NotFound();
                }
                return PartialView("_EditTaskModal", taskType); // _EditTaskModal.cshtml döndürülüyor
            }
            return PartialView("_CreateNewTaskTypeModal", new TaskType { UserId = userId.Value, Order = 1, Color = "#000000" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdateTaskType(TaskType taskType)
        {
            Console.WriteLine($"CreateOrUpdateTaskType POST - Id: {taskType.Id}, Name: {taskType.Name}, Color: {taskType.Color}");
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            taskType.UserId = userId;

            if (string.IsNullOrEmpty(taskType.Color))
            {
                taskType.Color = "#000000"; // Boşsa varsayılan renk ata
            }

            if (taskType.Id == 0)
            {
                taskType.CreatedDate = DateTime.Now;
                _context.TaskTypes.Add(taskType);
            }
            else
            {
                var existingTaskType = await _context.TaskTypes.FindAsync(taskType.Id);
                if (existingTaskType == null || existingTaskType.UserId != userId)
                {
                    return Json(new { success = false, message = "TaskType bulunamadı veya yetkiniz yok." });
                }
                existingTaskType.Name = taskType.Name;
                existingTaskType.Description = taskType.Description;
                existingTaskType.Color = taskType.Color;
                existingTaskType.Order = taskType.Order;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                return Json(new { success = false, message = "İşlem sırasında hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTaskType(int id, string name, string color)
        {
            var taskType = await _context.TaskTypes.FindAsync(id);
            if (taskType == null)
                return Json(new { success = false, message = "TaskType bulunamadı." });

            Console.WriteLine($"UpdateTaskType - Id: {id}, Name: {name}, Color: {color}");
            taskType.Name = name;
            taskType.Color = string.IsNullOrEmpty(color) ? "#000000" : color; // Boşsa varsayılan renk ata
            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateTaskType Error: {ex.Message}");
                return Json(new { success = false, message = "Güncelleme sırasında hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTaskType(int id)
        {
            var taskType = await _context.TaskTypes.Include(t => t.Tasks).FirstOrDefaultAsync(t => t.Id == id);
            if (taskType == null)
                return Json(new { success = false, message = "TaskType bulunamadı." });

            if (taskType.Tasks.Any())
                return Json(new { success = false, message = "Bu TaskType'a bağlı görevler var, önce onları silin veya taşıyın." });

            _context.TaskTypes.Remove(taskType);
            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme sırasında hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTaskTypeId(int taskId, int newTaskTypeId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return Json(new { success = false, message = "Görev bulunamadı." });

            task.TaskTypeId = newTaskTypeId;
            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Güncelleme sırasında hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return Json(new { success = false, message = "Görev bulunamadı." });

            _context.Tasks.Remove(task);
            try
            {
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme sırasında hata oluştu: " + ex.Message });
            }
        }


        //User Profile Edit 

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier); // buada ClaimTypes.NameIdentifier kullanarak kullanıcı ID'sini alıyorum
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();
            var userId = int.Parse(userIdStr);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound();
            var viewModel = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserImage = user.UserImage
            };

            ViewBag.UserImage = user.UserImage;
            ViewBag.UserFirstandLastName = $"{user?.FirstName} {user?.LastName}";
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model, UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();
            var userId = int.Parse(userIdStr);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound();



            string newFileName = user.UserImage; // Eğer kullanıcı yeni bir dosya yüklemediyse, eski dosya adını kullanıyoruz.
            if (userDto.UserImage != null) // Eğer kullanıcı yeni bir dosya yüklediyse, eski dosyayı silip, yeni dosyayı yüklüyoruz.
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(userDto.UserImage!.FileName); 
                string imageFullPath = environment.WebRootPath + "/assets/userImage/" + newFileName; 
                using (var stream = System.IO.File.Create(imageFullPath)) 
                {
                    userDto.UserImage.CopyTo(stream); // Dosyayı yeni dosyaya kopyalıyoruz.
                }

                string oldImagePath = environment.WebRootPath + "/assets/userImage/" + user.UserImage;// Eski dosyanın yolu
                System.IO.File.Delete(oldImagePath); // Eski dosyayı siliyoruz.
            }







            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.UserImage = newFileName; // burada yeni dosya adını veritabanına kaydediyoruz.
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Tasks");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            
            return View(model);
        }


        public IActionResult Settings()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier); // buada ClaimTypes.NameIdentifier kullanarak kullanıcı ID'sini alıyorum


            return View();
        }

    }
}