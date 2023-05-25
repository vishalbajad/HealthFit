using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthFit.Object_Provider.Model
{
    [Serializable]
    public class Journal
    {
        private int _JournalID;
        [ValidateNever]
        public int JournalID { get { return _JournalID; } set { _JournalID = value; } }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters exceeded")]
        public string? Title { get; set; }

        [Required]
        public string? ISSN { get; set; }

        [ValidateNever]
        public int PublisherID { get; set; }

        private string _PublicationFrequency = string.Empty;
        [Required]
        [Display(Name = "Publication Frequency")]
        public string? PublicationFrequency { get { return _PublicationFrequency; } set { _PublicationFrequency = value; } }

        private string _category = string.Empty;
        [Required]
        [Display(Name = "Category")]
        public string? Category { get { return _category; } set { _category = value; } }

        [Required]
        [Display(Name = "Publication Year")]
        public string? PublicationStartYear { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        private string _subjectArea = string.Empty;
        [Display(Name = "Subject Area")]
        [ValidateNever]
        public string? SubjectArea { get { return _subjectArea; } set { _subjectArea = value; } }

        private string _ImpactFactor = string.Empty;
        [Display(Name = "Impatct Factor")]
        [ValidateNever]
        public string? ImpactFactor { get { return _ImpactFactor; } set { _ImpactFactor = value; } }

        private string _website;
        [ValidateNever]
        public string? Website { get { return _website; } set { _website = value; } }

        private string _EditorialBoard = string.Empty;
        [Display(Name = "Editorial Board")]
        [ValidateNever]
        public string? EditorialBoard { get { return _EditorialBoard; } set { _EditorialBoard = value; } }

        private string _IndexingInformation = string.Empty;
        [Display(Name = "Indexing Information")]
        [ValidateNever]
        public string? IndexingInformation { get { return _IndexingInformation; } set { _IndexingInformation = value; } }

        private string _Format = string.Empty;
        [ValidateNever]
        public string? Format { get { return _Format; } set { _Format = value; } }

        private string _CitationMetrics = string.Empty;
        [Display(Name = "Citation Metrics")]
        [ValidateNever]
        public string? CitationMetrics { get { return _CitationMetrics; } set { _CitationMetrics = value; } }

        private string _SubmissionGuidelines = string.Empty;
        [Display(Name = "Submission Guidelines")]
        [ValidateNever]
        public string? SubmissionGuidelines { get { return _SubmissionGuidelines; } set { _SubmissionGuidelines = value; } }

        private string _Rating;
        [ValidateNever]
        public string? Rating { get { return _Rating; } set { _Rating = value; } }

        private string _JournalCoverPhotoPath = string.Empty;
        [Display(Name = "Journal Cover Photo")]
        [ValidateNever]
        public string? JournalCoverPhotoPath { get { return _JournalCoverPhotoPath; } set { _JournalCoverPhotoPath = value; } }

        private string _JournalPdfPath = string.Empty;
        [Display(Name = "Journal File (.pdf)")]
        [ValidateNever]
        public string? JournalPdfPath { get { return _JournalPdfPath; } set { _JournalPdfPath = value; } }

        private byte[] _JournalPdfPathByte;
        [NotMapped]
        [ValidateNever]
        public byte[] JournalPdfPathByte { get { return _JournalPdfPathByte; } set { _JournalPdfPathByte = value; } }

        private string _JournalCoverPhotoPathByte = string.Empty;
        [NotMapped]
        [ValidateNever]
        public string? JournalCoverPhotoPathByte { get { return _JournalCoverPhotoPathByte; } set { _JournalCoverPhotoPathByte = value; } }

        private bool _isActive = true;
        [ValidateNever]
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }

        [ValidateNever]
        [NotMapped]
        public ICollection<UserSubscriptionsDetails> Subscribers_UserSubscriptionsDetails { get; set; }

        [ValidateNever]
        [NotMapped]
        public List<User> Subscribers { get; set; }
    }

}
