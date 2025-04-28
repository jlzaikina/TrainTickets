using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainTickets.UI.Entities;
[Table("Schema", Schema = "public")]
public class SchemaEntity
{
    [Key]
    public int Id_schema { get; set; }

    [Column(TypeName = "jsonb")]
    public string Schema { get; set; }
}
