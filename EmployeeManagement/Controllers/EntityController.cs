using Base.API.Controllers;
using EmployeeManagement.Data;
using EmployeeManagement.Manager;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using EntityType = EmployeeManagement.Models.EntityType;

namespace EmployeeManagement.Controllers
{
    [ApiController, Route("[controller]/[action]")]
    public class EntityController : BaseController
    {
        private readonly EntityTypeManager _entityTypeManager;
        private readonly EntityValueManager _entityValueManager;
        private readonly DbContextClass _dbContext;

        public EntityController(DbContextClass dbContext)
        {
            _dbContext = dbContext;
            _entityTypeManager = new EntityTypeManager(dbContext);
            _entityValueManager = new EntityValueManager(dbContext);
        }
        // Entity Type:  Region *******************************
        // GET: api/GetEntityType
        [HttpGet]
        public IActionResult GetAllEntityType()
        {
            try
            {
                var sql = "Select * from EntityType";
                var AllData = _entityValueManager.ExecuteRawSql(sql);
                List<EntityType> entities = new List<EntityType>();
                entities = (from DataRow dtrow in AllData.Rows
                            select new EntityType()
                            {
                                Id = Convert.ToInt32(dtrow["Id"]),
                                Name = dtrow["Name"].ToString()
                            }).ToList();
                return Ok(entities);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet("{id}")]
        public IActionResult GetEntityTypeById(int id)
        {
            try
            {
                var existData = _entityTypeManager.GetById(id);
                if (existData == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(existData);
                }
                
            }
            catch ( Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //return Ok();
            
        }


        [HttpPost]
        public IActionResult AddEntityType(EntityType entityType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else
                {
                  bool result = _entityTypeManager.Add(entityType);
                    if (result)
                    {
                        return Ok("Entity is added successfully.");
                    }
                    else
                    {
                        return BadRequest("Entity save failed.");
                    }
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult EditEntityType(EntityType entity,int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else
                {
                    var existingData = _entityTypeManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    existingData.Name = entity.Name;
                   bool result= _entityTypeManager.Update(existingData);
                    if(result)
                    {
                        return Ok("Entity is Updated successfully.");
                    }
                    else
                    {
                        return BadRequest($"{existingData.Name} not found");
                    }
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }                      
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEntityType(int id)
        {
            try { 
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var existingData = _entityTypeManager.GetById(id);
                if (existingData == null)
                {
                    return NotFound();
                }
               bool result= _entityTypeManager.Delete(existingData);
                if (result)
                {
                    return Ok("Deleted Entity Type successfully");
                }
                else
                {
                    return BadRequest("Failed to delete Entity Type");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }


        // Start Region Entity Value *****************

        [HttpGet]
        public IActionResult GetAllEntityValue()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var sql = "Select v.Id as EntityValueId,v.EntityId as EntityTypeId,v.[Value],t.Name as EntityType from EntityValue AS v INNER JOIN EntityType t ON v.EntityId=t.Id";
                var allEntityValue = _entityValueManager.ExecuteRawSql(sql);
                List<VmEntityValue> valueList = new List<VmEntityValue>();
                valueList = (from DataRow dtRow in allEntityValue.Rows
                             select new VmEntityValue
                             {
                                 EntityValueId = dtRow["EntityValueId"].ToString(),
                                 EntityTypeId = dtRow["EntityTypeId"].ToString(),
                                 EntityValue = dtRow["Value"].ToString(),
                                 EntityType = dtRow["EntityType"].ToString(),

                             }).ToList();

                return Ok(valueList);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet("{id}")]
        public IActionResult GetEntityValueById(int id)
        {
            try
            {
                var existData = _entityValueManager.GetById(id);
                if (existData == null)
                {
                    return NotFound();
                }
                return Ok(existData);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPost]
        public IActionResult AddEntityValue(EntityValue entityValue)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    if(entityValue == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        List<EntityType> entities = new List<EntityType>();
                        var entitylist=_entityTypeManager.GetAll();
                        //List<EntityValue> valuelist = new List<EntityValue>();
                        //foreach (var e in entitylist)
                        //{
                        //    entityValue.EntityId=e.Id;
                        //    entityValue.Value=entityValue.Value;
                        //    valuelist.Add(entityValue);
                        //}
                        bool result=  _entityValueManager.Add(entityValue);
                        if (result)
                        {
                            return Ok("Add Entity Value Successfully");
                        }
                        else
                        {
                            return BadRequest("Entity Value Added Failed");
                        }
                    }
                    
                }catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
           
        }
        [HttpPut("{id}")]
        public IActionResult EditEntityValue(EntityValue entity,int id)
        {
            if (ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var existingData = _entityValueManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        existingData.EntityId= entity.EntityId;
                        existingData.Value= entity.Value;
                       bool result= _entityValueManager.Update(existingData);
                        if(result)
                        {
                            return Ok("Entity Value Updated Successfully.");
                        }
                        else
                        {
                            return BadRequest("Entity Value Updated Fail!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest (ex.Message);
                }
            }
            
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEntityValue(int id)
        {
            if(ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    var existingData = _entityValueManager.GetById(id);
                    if (existingData == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                      bool result=  _entityValueManager.Delete(existingData);
                        if (result)
                        {
                            return Ok("Deleted Entity value successfully");
                        }
                        else
                        {
                            return BadRequest("Failed to Delete Entity Value");
                        }
                    }

                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        }


    }
}
