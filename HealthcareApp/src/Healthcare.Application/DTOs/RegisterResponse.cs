using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthcare.Application.DTOs;

public record RegisterResponse(bool Flag, string Massege = null!, string Token = null!);
