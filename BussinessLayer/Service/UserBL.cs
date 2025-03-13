using System;
using BussinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Helper;
using RepositoryLayer.Interface;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BussinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly PasswordHashing _PasswordHashing;
        private readonly jwtHelper _jwtHelper;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserBL> _logger;

        public UserBL(IUserRL userRL, PasswordHashing PasswordHashing, jwtHelper JwtHelper, IEmailService emailService, ILogger<UserBL> logger)
        {
            _userRL = userRL;
            _PasswordHashing = PasswordHashing;
            _jwtHelper = JwtHelper;
            _emailService = emailService;
            _logger = logger;
        }

        public bool Register(RegisterDTO model)
        {
            try
            {
                _logger.LogInformation("Register method started for email: {Email}", model.Email);

                var existingUser = _userRL.GetUserByEmail(model.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("User already registered: {Email}", model.Email);
                    throw new Exception("User already registered.");
                }

                byte[] hashedPassword = PasswordHashing.HashPassword(model.Password);

                UsersEntity user = new()
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    JwtToken = null
                };

                bool result = _userRL.Register(user);
                _logger.LogInformation("User registration {Status} for email: {Email}", result ? "successful" : "failed", model.Email);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Register method for email: {Email}", model.Email);
                throw;
            }
        }

        public string Login(LoginDTO model)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", model.Email);

                var user = _userRL.GetUserByEmail(model.Email);
                if (user == null || !PasswordHashing.VerifyPassword(model.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Invalid login attempt for email: {Email}", model.Email);
                    return null;
                }

                string token = _jwtHelper.GenerateToken(user);
                _userRL.SaveToken(user.Id, token);

                _logger.LogInformation("Login successful for email: {Email}", model.Email);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Login method for email: {Email}", model.Email);
                throw;
            }
        }

        public bool ForgotPassword(string email)
        {
            try
            {
                _logger.LogInformation("ForgotPassword request for email: {Email}", email);

                var user = _userRL.GetUserByEmail(email);
                if (user == null)
                {
                    _logger.LogWarning("ForgotPassword request failed. Email not found: {Email}", email);
                    return false;
                }

                string resetToken = _jwtHelper.GenerateResetToken(user);
                _logger.LogInformation("Generated Reset Token: {token}", resetToken);
                _userRL.SaveResetToken(user.Id, resetToken);

                string resetLink = $"https://V.com/resetpassword?token={resetToken}";
                string subject = "Password Reset Request";
                string body = resetToken;
                bool emailSent = _emailService.SendEmail(user.Email, subject, body);
                _logger.LogInformation("Password reset email {Status} for email: {Email}", emailSent ? "sent successfully" : "failed to send", email);

                return emailSent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ForgotPassword method for email: {Email}", email);
                throw;
            }
        }

        public UsersEntity GetUserByEmail(string email)
        {
            _logger.LogInformation("Fetching user details for email: {Email}", email);
            return _userRL.GetUserByEmail(email);
        }

        public void SaveResetToken(int userId, string resetToken)
        {
            _logger.LogInformation("Saving reset token for user ID: {UserId}", userId);
            _userRL.SaveResetToken(userId, resetToken);
        }


        public bool ResetPassword(string token, string newPassword)
        {
            try
            {
                var principal = _jwtHelper.ValidateToken(token);
                if (principal == null)
                {
                    _logger.LogWarning("Invalid or expired token.");
                    return false;
                }

                int userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0)
                {
                    _logger.LogWarning("User ID not found in token.");
                    return false;
                }

                byte[] newPasswordHash = PasswordHashing.HashPassword(newPassword);
                return _userRL.ResetPassword(userId, newPasswordHash);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ResetPassword method: {ex.Message}");
                throw;
            }
        }

        public UsersEntity? ValidateResetToken(string token)
        {
            var decodedToken = _jwtHelper.ValidateToken(token);
            if (decodedToken == null)
                return null;


            int userId = int.Parse(decodedToken.Claims.First(c => c.Type == "UserId").Value);
            var user = _userRL.GetUserById(userId);

            if (user == null || user.ResetToken != token || user.ResetTokenExpiry < DateTime.UtcNow)
                return null;

            return user;
        }

        public bool UpdatePassword(int userId, byte[] newPasswordHash)
        {
            return _userRL.UpdatePassword(userId, newPasswordHash);
        }


    }
}
