using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Model;
using System.Collections;
using System.Reflection;
using System.Text;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParserController : ControllerBase
    {
        [HttpPost, Route("Parse")]
        public ActionResult TryParse(Rootobject glossary)
        {
            if (glossary == null)
            {
                return NotFound("Обьект не найден");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                PrintGlossaryObject(glossary, sb);
                return Ok(sb.ToString());
            }

        }

        private void PrintGlossaryObject(object obj, StringBuilder output, int depth = 0)
        {
            if (obj == null)
                return;

            string indent = new string(' ', depth * 2);

            if (obj is IEnumerable enumerable && !(obj is string))
            {
                foreach (var item in enumerable)
                {
                    PrintGlossaryObject(item, output, depth + 1);
                }
            }
            else
            {
                var properties = obj.GetType().GetProperties();
                foreach (var property in properties)
                {
                    object value;
                    try
                    {
                        value = property.GetValue(obj);
                    }
                    catch (TargetParameterCountException)
                    {
                        continue;
                    }

                    if (value is IEnumerable collection && !(value is string))
                    {
                        output.AppendLine($"{indent}{property.Name}:");
                        PrintGlossaryObject(collection, output, depth + 1);
                    }
                    else if (value != null && value.GetType().IsClass && !(value is string))
                    {
                        output.AppendLine($"{indent}{property.Name}:");
                        PrintGlossaryObject(value, output, depth + 1);
                    }
                    else
                    {
                        output.AppendLine($"{indent}{property.Name}: {value}");
                    }
                }
            }
        }
    }
}

