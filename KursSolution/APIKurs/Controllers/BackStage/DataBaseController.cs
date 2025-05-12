using APIKurs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Runtime.InteropServices;

namespace APIKurs.Controllers.BackStage
{
    public class DataBaseController : ControllerBase
    {
        public static DataBaseController Instance { get => instance ??= new(); }

        private static DataBaseController instance;
        private QwertyContext _context;
        DataBaseController()
            => _context = new();

        //System Metods
        private async Task Save()
            => await _context.SaveChangesAsync();

        private async Task Update()
            => _context = new();

        //Security Methods
        private string EncryptPassword(string someString)
        {
            someString = BasePasswordEncode(someString);
            someString = PeperEncrypting(someString);

            return someString;
        }

        private static string BasePasswordEncode(string someString)
        {
            var bytes = Encoding.UTF8.GetBytes(someString);
            bytes = SHA256.HashData(bytes);

            return Convert.ToHexString(bytes).ToLower();
        }

        private string EncryptToken(string someString)
            => PeperEncrypting(someString);

        private string DecryptToken(string someString)
            => PeperDecrypting(someString);

        private string PeperDecrypting(string someString)
        {
            var nums = System.IO.File.ReadAllText("Models/NumsFile.txt").Split(';', StringSplitOptions.RemoveEmptyEntries);

            var array = someString.ToCharArray().ToList();

            // Этап 2: тесто отдыхает (обратный этап)
            var boofCh = array[int.Parse(nums[1])];
            array[int.Parse(nums[1])] = array[int.Parse(nums[0])];
            array[int.Parse(nums[0])] = boofCh;
            var count = 0;
            // Этап 1: замес теста (обратный этап)
            for (int i = 0; i < array.Count - 3; i += 3)
                count += 3;

            for (int i = count; i >= 3; i -= 3)
                (array[i - 3], array[i]) = (array[i], array[i - 3]);

            return new string(array.ToArray());
        }

        private string PeperEncrypting(string someString)
        {
            var nums = System.IO.File.ReadAllText("Models/NumsFile.txt").Split(';', StringSplitOptions.RemoveEmptyEntries);
            var rnd = new Random();

            var array = someString.ToCharArray().ToList();

            //Этап 1: замес теста
            for (int i = 0; i < array.Count - 3; i += 3)
                (array[i], array[i + 3]) = (array[i + 3], array[i]);

            //Этап 2: тесто отдыхает
            var boofCh = array[int.Parse(nums[0])];

            array[int.Parse(nums[0])] = array[int.Parse(nums[1])];
            array[int.Parse(nums[1])] = boofCh;

            return new string(array.ToArray());
        }

        //Model Methods

        //Enter
        public async Task<ActionResult<TokEnRole>> Authorise(string someString, string otherString)
        {
            Update();
            List<User> a = _context.Users.ToList();
            User? user = _context.Users.FirstOrDefault(u => u.Login == someString && u.Password == EncryptPassword(otherString));
            if (user is null)
                return NotFound();

            var role = await _context.Roles.FirstOrDefaultAsync(s => s.Id == user.IdRole);
            int? id = user.Id;

            var claims = new List<Claim> {
                 new Claim(ClaimValueTypes.Integer32, id.ToString()),
                 new Claim(ClaimTypes.Role, role.Ttle)
            };

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            var result = new TokEnRole() { Title = role, Token = token };

            return result;
        }

        public async Task<ActionResult<TokEnRole>> Register(User user)
        {
            var role = _context.Roles.First(s => s.Ttle == "1");
            user = new User() { Login = user.Login, Password = EncryptPassword(user.Password), IdRoleNavigation = role, IdRole = role.Id };
            await _context.Users.AddAsync(user);
            await Save();
            await Update();
            return new TokEnRole();
        }

        //Conditions

        public async Task<ActionResult<IEnumerable<Condition>>> GetConditions()
            => _context.Conditions.ToList();

        public async Task<ActionResult<Condition>> GetCondition(int id)
        {
            var condition = await _context.Conditions.FindAsync(id);

            if (condition == null)
            {
                return NotFound();
            }

            return condition;
        }

        public async Task<IActionResult> PutCondition(int id, Condition condition)
        {
            if (id != condition.Id)
            {
                return BadRequest();
            }

            try
            {
                //_context.Entry(condition).State = EntityState.Modified;
                var local = _context.Set<Models.Condition>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(condition.Id));
                if (local is not null)
                    _context.Entry(local).State = EntityState.Detached;
                _context.Entry(condition).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConditionExists(id))
                    return NotFound();
            }
            return NoContent();
        }

        public async Task<ActionResult<Condition>> PostCondition(Condition condition)
        {
            _context.Conditions.Add(condition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCondition", new { id = condition.Id }, condition);
        }

        public async Task<IActionResult> DeleteCondition(int id)
        {
            var condition = await _context.Conditions.FindAsync(id);
            if (condition == null)
            {
                return NotFound();
            }

            _context.Conditions.Remove(condition);
            await _context.SaveChangesAsync();

            if (_context.Conditions.Contains(condition))
                return NoContent();
            return Ok();
        }

        private bool ConditionExists(int id)
        {
            return _context.Conditions.Any(e => e.Id == id);
        }

        //People

        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }

        //Posts

        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        //Ppe

        public async Task<ActionResult<IEnumerable<Ppe>>> GetPpes()
        {
            return await _context.Ppes.ToListAsync();
        }

        public async Task<ActionResult<Ppe>> GetPpe(int id)
        {
            var ppe = await _context.Ppes.FindAsync(id);

            if (ppe == null)
            {
                return NotFound();
            }

            return ppe;
        }

        public async Task<IActionResult> PutPpe(int id, Ppe ppe)
        {
            if (id != ppe.Id)
            {
                return BadRequest();
            }

            _context.Entry(ppe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PpeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public async Task<ActionResult<Ppe>> PostPpe(Ppe ppe)
        {
            _context.Ppes.Add(ppe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPpe", new { id = ppe.Id }, ppe);
        }

        public async Task<IActionResult> DeletePpe(int id)
        {
            var ppe = await _context.Ppes.FindAsync(id);
            if (ppe == null)
            {
                return NotFound();
            }

            _context.Ppes.Remove(ppe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PpeExists(int id)
        {
            return _context.Ppes.Any(e => e.Id == id);
        }

        //PpeType

        public async Task<ActionResult<IEnumerable<PpeType>>> GetPpeTypes()
        {
            return await _context.PpeTypes.ToListAsync();
        }

        public async Task<ActionResult<PpeType>> GetPpeType(int id)
        {
            var ppeType = await _context.PpeTypes.FindAsync(id);

            if (ppeType == null)
            {
                return NotFound();
            }

            return ppeType;
        }

        public async Task<IActionResult> PutPpeType(int id, PpeType ppeType)
        {
            if (id != ppeType.Id)
            {
                return BadRequest();
            }

            _context.Entry(ppeType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PpeTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public async Task<ActionResult<PpeType>> PostPpeType(PpeType ppeType)
        {
            _context.PpeTypes.Add(ppeType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPpeType", new { id = ppeType.Id }, ppeType);
        }

        public async Task<IActionResult> DeletePpeType(int id)
        {
            var ppeType = await _context.PpeTypes.FindAsync(id);
            if (ppeType == null)
            {
                return NotFound();
            }

            _context.PpeTypes.Remove(ppeType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PpeTypeExists(int id)
        {
            return _context.PpeTypes.Any(e => e.Id == id);
        }

        //Status

        public async Task<ActionResult<IEnumerable<Status>>> GetStatuses()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<ActionResult<Status>> GetStatus(int id)
        {
            var status = await _context.Statuses.FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return status;
        }

        public async Task<IActionResult> PutStatus(int id, Status status)
        {
            if (id != status.Id)
            {
                return BadRequest();
            }

            _context.Entry(status).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public async Task<ActionResult<Status>> PostStatus(Status status)
        {
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatus", new { id = status.Id }, status);
        }

        public async Task<IActionResult> DeleteStatus(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }

            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatusExists(int id)
        {
            return _context.Statuses.Any(e => e.Id == id);
        }

        //User

        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}