using System;
using System.Linq;
using System.Text.RegularExpressions;
using NewHabr.Domain.Models;

namespace NewHabr.Business.Extensions;

public static class StringExtensions
{
    internal static ICollection<string> FindMentionedUsers(this string text)
    {
        string pattern = @"(?<=@)[A-Za-z0-9]{3,}\b";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        var roles = Enum.GetNames(typeof(UserRoles));
        var usernames = regex
            .Matches(text)
            .Select(m => m.Value)
            .Except(roles, StringComparer.OrdinalIgnoreCase)
            .ToList();
        return usernames;
    }
}

