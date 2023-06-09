﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PresseMots_DataAccess.Context;

#nullable disable

namespace PresseMots_DataAccess.Migrations
{
    [DbContext(typeof(PresseMotsDbContext))]
    [Migration("20220929204323_initData")]
    partial class initData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PresseMots_DataModels.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2500)
                        .HasColumnType("nvarchar(2500)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Hidden")
                        .HasColumnType("bit");

                    b.Property<int>("StoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoryId");

                    b.ToTable("Comments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "Je trouve cet article un peu superficiel",
                            DisplayName = "Thierry",
                            Email = "t.girouxveilleux@cegepmontpetit.ca",
                            Hidden = false,
                            StoryId = 1
                        },
                        new
                        {
                            Id = 2,
                            Content = "Je suis d'accord",
                            DisplayName = "Valérie",
                            Email = "v.turgeon@cegepmontpetit.ca",
                            Hidden = false,
                            StoryId = 1
                        },
                        new
                        {
                            Id = 3,
                            Content = "C'est bizarre comme article. Ça ressemble au =rand() de word...",
                            DisplayName = "Thierry",
                            Email = "t.girouxveilleux@cegepmontpetit.ca",
                            Hidden = false,
                            StoryId = 2
                        });
                });

            modelBuilder.Entity("PresseMots_DataModels.Entities.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("StoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmittedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("PresseMots_DataModels.Entities.Share", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("StoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmittedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Shares");
                });

            modelBuilder.Entity("PresseMots_DataModels.Entities.Story", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Draft")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastEditTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PublishTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Stories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.\n\nNunc viverra imperdiet enim. Fusce est. Vivamus a tellus.\n\nPellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.\n\nAenean nec lorem. In porttitor. Donec laoreet nonummy augue.\n\nSuspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.\n",
                            CreationTime = new DateTime(2021, 10, 4, 13, 12, 0, 0, DateTimeKind.Unspecified),
                            Draft = false,
                            OwnerId = 1,
                            Title = "Première publication"
                        },
                        new
                        {
                            Id = 2,
                            Content = "Les vidéos vous permettent de faire passer votre message de façon convaincante. Quand vous cliquez sur Vidéo en ligne, vous pouvez coller le code incorporé de la vidéo que vous souhaitez ajouter. Vous pouvez également taper un mot-clé pour rechercher en ligne la vidéo qui convient le mieux à votre document.\n\nPour donner un aspect professionnel à votre document, Word offre des conceptions d’en-tête, de pied de page, de page de garde et de zone de texte qui se complètent mutuellement. Vous pouvez par exemple ajouter une page de garde, un en-tête et une barre latérale identiques. Cliquez sur Insérer et sélectionnez les éléments de votre choix dans les différentes galeries.\n\nLes thèmes et les styles vous permettent également de structurer votre document. Quand vous cliquez sur Conception et sélectionnez un nouveau thème, les images, graphiques et SmartArt sont modifiés pour correspondre au nouveau thème choisi. Quand vous appliquez des styles, les titres changent pour refléter le nouveau thème.\n\nGagnez du temps dans Word grâce aux nouveaux boutons qui s'affichent quand vous en avez besoin. Si vous souhaitez modifier la façon dont une image s’ajuste à votre document, cliquez sur celle-ci pour qu’un bouton d’options de disposition apparaisse en regard de celle-ci. Quand vous travaillez sur un tableau, cliquez à l’emplacement où vous souhaitez ajouter une ligne ou une colonne, puis cliquez sur le signe plus.\n\nLa lecture est également simplifiée grâce au nouveau mode Lecture. Vous pouvez réduire certaines parties du document et vous concentrer sur le texte désiré. Si vous devez stopper la lecture avant d’atteindre la fin de votre document, Word garde en mémoire l’endroit où vous avez arrêté la lecture, même sur un autre appareil\n\n",
                            CreationTime = new DateTime(2021, 10, 4, 13, 14, 0, 0, DateTimeKind.Unspecified),
                            Draft = true,
                            OwnerId = 1,
                            Title = "Utilité de MS Word"
                        });
                });

            modelBuilder.Entity("PresseMots_DataModels.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "snoopy@peanuts.com",
                            Username = "Snoopy"
                        });
                });

            modelBuilder.Entity("PresseMots_DataModels.Entities.Comment", b =>
                {
                    b.HasOne("PresseMots_DataModels.Entities.Story", "Story")
                        .WithMany("Comments")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Story");
                });

            modelBuilder.Entity("PresseMots_DataModels.Entities.Like", b =>
                {
                    b.HasOne("PresseMots_DataModels.Entities.Story", "Story")
                        .WithMany("Likes")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PresseMots_DataModels.Entities.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Story");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PresseMots_DataModels.Entities.Share", b =>
                {
                    b.HasOne("PresseMots_DataModels.Entities.Story", "Story")
                        .WithMany("Shares")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PresseMots_DataModels.Entities.User", "User")
                        .WithMany("Shares")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Story");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PresseMots_DataModels.Entities.Story", b =>
                {
                    b.HasOne("PresseMots_DataModels.Entities.User", "Owner")
                        .WithMany("Stories")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("PresseMots_DataModels.Entities.Story", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("Shares");
                });

            modelBuilder.Entity("PresseMots_DataModels.Entities.User", b =>
                {
                    b.Navigation("Likes");

                    b.Navigation("Shares");

                    b.Navigation("Stories");
                });
#pragma warning restore 612, 618
        }
    }
}
