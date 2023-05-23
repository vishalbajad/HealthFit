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
        public int JournalID { get { return _JournalID; } set { _JournalID = value; } }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters exceeded")]
        public string Title { get; set; }
        [Required]
        public string ISSN { get; set; }
        [Required]
        public int PublisherID { get; set; }

        private string _PublicationFrequency = string.Empty;
        [Required]
        [Display(Name = "Publication Frequency")]
        public string PublicationFrequency { get { return _PublicationFrequency; } set { _PublicationFrequency = value; } }

        [Required]
        [Display(Name = "Publication Year")]
        public int PublicationStartYear { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        private string _subjectArea = string.Empty;
        [Display(Name = "Subject Area")]
        public string SubjectArea { get { return _subjectArea; } set { _subjectArea = value; } }

        private decimal _ImpactFactor = 0;
        [Display(Name = "Impatct Factor")]
        public decimal ImpactFactor { get { return _ImpactFactor; } set { _ImpactFactor = value; } }

        private string _website;
        public string Website { get { return _website; } set { _website = value; } }

        private string _EditorialBoard = string.Empty;
        [Display(Name = "Editorial Board")]
        public string EditorialBoard { get { return _EditorialBoard; } set { _EditorialBoard = value; } }

        private string _IndexingInformation = string.Empty;
        [Display(Name = "Indexing Information")]
        public string IndexingInformation { get { return _IndexingInformation; } set { _IndexingInformation = value; } }

        private string _Format = string.Empty;
        public string Format { get { return _Format; } set { _Format = value; } }

        private string _CitationMetrics = string.Empty;
        [Display(Name = "Citation Metrics")]
        public string CitationMetrics { get { return _CitationMetrics; } set { _CitationMetrics = value; } }

        private string _SubmissionGuidelines = string.Empty;
        [Display(Name = "Submission Guidelines")]
        public string SubmissionGuidelines { get { return _SubmissionGuidelines; } set { _SubmissionGuidelines = value; } }

        private decimal _Rating;
        public decimal Rating { get { return _Rating; } set { _Rating = value; } }

        private string _JournalCoverPhotoPath = string.Empty;
        [Display(Name = "Journal Cover Photo")]
        public string JournalCoverPhotoPath { get { return _JournalCoverPhotoPath; } set { _JournalCoverPhotoPath = value; } }

        private string _JournalPdfPath = string.Empty;
        [Display(Name = "Journal File (.pdf)")]
        public string JournalPdfPath { get { return _JournalPdfPath; } set { _JournalPdfPath = value; } }

        private bool _isActive = true;
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    }

}
