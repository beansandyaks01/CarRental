using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using HajurKoCarRental.Data;
using HajurKoCarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using System.ComponentModel;
using HajurKoCarRental.DTOs.VehicleDTOs;

namespace HajurKoCarRental.Controllers
{
    [Route("vehicle/")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private Account _account;
        private Cloudinary _cloudinary;
        private readonly ApplicationDbContext _dbContext;
       
        public VehicleController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _account = new Account { Cloud = "dnow8alub", ApiKey = "719549538397893", ApiSecret = "9LasXvfPK7TrvufQjkk4Q7ZGfhs" };
            _cloudinary = new Cloudinary(_account);
        }


        [HttpGet("view-vehicles")]
        public async Task<IActionResult> GetAllVehicles()
        {
            try
            {
                var response = await _dbContext.Vehicles.ToListAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       

        [HttpPost("add-vehicles")]
        public async Task<IActionResult> AddVehicle(InsertVehicleDTO data)
        {
            var newVehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Name = data.Name,
                Type = data.Type,
                Brand = data.Brand,
                NoOfSeat = data.NoOfSeat,
                VehicleNo = data.VehicleNo,
                RentPerDay = data.RentPerDay,
                FuelType = data.FuelType,
            };
            _dbContext.Vehicles.Add(newVehicle);
            _dbContext.SaveChanges();
            return Ok(newVehicle);
        }

        [HttpGet("view-vehicle-{id}")]
        public async Task<IActionResult> GetVehicleById(Guid id)
        {
            var response = await _dbContext.Vehicles.FindAsync(id);
            return Ok(response);
        }

        

        [HttpPatch("update-vehicle-{id}")]
        public async Task<IActionResult> UpdateVehicleInfo(Guid id, UpdateVehicleDTO data)
        {
            try
            {
                var vehicle = await _dbContext.Vehicles.FindAsync(id);
                if (vehicle == null)
                {
                    throw new Exception("Invalid vehicle id");
                }
                vehicle.Name = data.Name;
                vehicle.Type = data.Type;
                vehicle.Brand = data.Brand;
                vehicle.NoOfSeat = data.NoOfSeat;
                vehicle.VehicleNo = data.VehicleNo;
                vehicle.RentPerDay = data.RentPerDay;
                vehicle.FuelType = data.FuelType;
                _dbContext.Vehicles.Update(vehicle);
                _dbContext.SaveChanges();
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveVehicle(Guid id)
        {
            try
            {
                var vehicle = await _dbContext.Vehicles.FindAsync(id);
                if(vehicle == null)
                {
                    throw new Exception("Invalid vehicle id");
                }
                _dbContext.Vehicles.Remove(vehicle);
                _dbContext.SaveChanges();
                return Ok("Suucessfully deleted");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload-vehicle-image")]
        public async Task<IActionResult> UploadVehicleImage(Guid vehicleId, IFormFile file)
        {
            if (file == null)
            {
                throw new Exception("Vehicle Image not found");

            }
            // Check if the file is in the allowed format
            if (file.ContentType != "image/png" && file.ContentType != "application/pdf")
            {
                return BadRequest("Invalid file format. Only PNG and PDF are allowed.");
            }
            // Check if the file size is within the limit
            if (file.Length > 1.5 * 1024 * 1024)
            {
                return BadRequest("File size exceeds the limit of 1.5 MB.");
            }

            var vehicle = await _dbContext.Vehicles.FindAsync(vehicleId);
 
            if (vehicle == null)
            {
                throw new Exception("Vehicle not found");
            }

            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            var imageId = Guid.NewGuid();
            
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = $"Vehicles/{imageId}",
                Transformation = new Transformation().FetchFormat("auto")
            };

            _cloudinary.Upload(uploadParams);
            
            var newVehicleImage = new VehicleImage
            {
                Id = Guid.NewGuid(),
                VehicleIMG = imageId,
                VehicleId = vehicleId
            };
            
            _dbContext.VehicleImages.Add(newVehicleImage);
            _dbContext.SaveChanges();
            return Ok("Vehicle Image added!");
        }

        
        [HttpGet("view-vehicle-image/{vehicleId}")]
        public async Task<IActionResult> ViewVehicleImage(Guid vehicleId)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(vehicleId);

            if (vehicle == null)
            {
                throw new Exception("Vehicle not found");
            }

            var vehicleImage = await _dbContext.VehicleImages.FirstOrDefaultAsync(vi => vi.VehicleId == vehicleId);

            if (vehicleImage == null)
            {
                throw new Exception("Vehicle Image not found");
            }

            var vehicleImageURL = _cloudinary.Api.UrlImgUp.Secure().BuildUrl($"Vehicles/{vehicleImage.VehicleIMG}.png");
            return Ok(vehicleImageURL);
        }
    }
}
