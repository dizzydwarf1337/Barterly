using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums.Posts
{
    public  enum PostStatusType
    {
        UnderReview = 0,  
        Published = 1,    
        Rejected = 2,     
        ReSubmitted = 3,   
        Deleted = 4,
    }
}
