using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthcare.Application.DTOs;

public record LoginResponse(bool Flag, string Massege = null!, string Token = null!);