using System;
using System.Collections.Generic;
using System.Diagnostics;
using Android.Content;
using Android.Provider;
using Uri = Android.Net.Uri;
using Microsoft.Xna.Framework.Media;

namespace MonoGame.Platform.AndroidGL.Media
{
	public partial class AndroidMediaLibraryPlatform : IMediaLibraryPlatform
    {
        private static readonly TimeSpan MinimumSongDuration = TimeSpan.FromSeconds(3);

		internal ContentResolver mResolver;

        private IAlbumCollection albumCollection;
        private ISongCollection songCollection;

		IAndroidAlbumArtContentResolver mArtResolver;

		public AndroidMediaLibraryPlatform (ContentResolver resolver, IAndroidAlbumArtContentResolver artResolver)
		{
			mResolver = resolver;
			mArtResolver = artResolver;
		}

		public void Initialize ()
		{

		}

		public void Dispose ()
		{

		}

        public void Load(Action<int> progressCallback)
        {
            List<ISong> songList = new List<ISong>();
            List<IAlbum> albumList = new List<IAlbum>();

			using (var musicCursor = mResolver.Query(MediaStore.Audio.Media.ExternalContentUri, null, null, null, null))
            {
                if (musicCursor != null)
                {
                    Dictionary<string, IArtist> artists = new Dictionary<string, IArtist>();
                    Dictionary<string, IAlbum> albums = new Dictionary<string, IAlbum>();
                    Dictionary<string, IGenre> genres = new Dictionary<string, IGenre>();

                    // Note: Grabbing album art using MediaStore.Audio.AlbumColumns.AlbumArt and
                    // MediaStore.Audio.AudioColumns.AlbumArt is broken
                    // See: https://code.google.com/p/android/issues/detail?id=1630
                    // Workaround: http://stackoverflow.com/questions/1954434/cover-art-on-android

                    int albumNameColumn = musicCursor.GetColumnIndex(MediaStore.Audio.AlbumColumns.Album);
                    int albumArtistColumn = musicCursor.GetColumnIndex(MediaStore.Audio.AlbumColumns.Artist);
                    int albumIdColumn = musicCursor.GetColumnIndex(MediaStore.Audio.AlbumColumns.AlbumId);
                    int genreColumn = musicCursor.GetColumnIndex(MediaStore.Audio.GenresColumns.Name); // Also broken :(

                    int artistColumn = musicCursor.GetColumnIndex(MediaStore.Audio.AudioColumns.Artist);
                    int titleColumn = musicCursor.GetColumnIndex(MediaStore.Audio.AudioColumns.Title);
                    int durationColumn = musicCursor.GetColumnIndex(MediaStore.Audio.AudioColumns.Duration);
                    int assetIdColumn = musicCursor.GetColumnIndex(MediaStore.Audio.AudioColumns.Id);

                    if (titleColumn == -1 || durationColumn == -1 || assetIdColumn == -1)
                    {
                        Debug.WriteLine("Missing essential properties from music library. Returning empty library.");
                        albumCollection = new StandardAlbumCollection(albumList);
                        songCollection = new StandardSongCollection(songList);
                        return;
                    }

                    for (musicCursor.MoveToFirst(); !musicCursor.IsAfterLast; musicCursor.MoveToNext())
                        try
                        {
                            long durationProperty = musicCursor.GetLong(durationColumn);
                            TimeSpan duration = TimeSpan.FromMilliseconds(durationProperty);

                            // Exclude sound effects
                            if (duration < MinimumSongDuration)
                                continue;

                            string albumNameProperty = (albumNameColumn > -1 ? musicCursor.GetString(albumNameColumn) : null) ?? "Unknown Album";
                            string albumArtistProperty = (albumArtistColumn > -1 ? musicCursor.GetString(albumArtistColumn) : null) ?? "Unknown Artist";
                            string genreProperty = (genreColumn > -1 ? musicCursor.GetString(genreColumn) : null) ?? "Unknown Genre";
                            string artistProperty = (artistColumn > -1 ? musicCursor.GetString(artistColumn) : null) ?? "Unknown Artist";
                            string titleProperty = musicCursor.GetString(titleColumn);

                            long assetId = musicCursor.GetLong(assetIdColumn);
                            var assetUri = ContentUris.WithAppendedId(MediaStore.Audio.Media.ExternalContentUri, assetId);
                            long albumId = albumIdColumn > -1 ? musicCursor.GetInt(albumIdColumn) : -1;
                            var albumArtUri = albumId > -1 ? ContentUris.WithAppendedId(Uri.Parse("content://media/external/audio/albumart"), albumId) : null;

                            IArtist artist;
                            if (!artists.TryGetValue(artistProperty, out artist))
                            {
								artist = new StandardArtist(artistProperty);
                                artists.Add(artist.Name, artist);
                            }

							IArtist albumArtist;
                            if (!artists.TryGetValue(albumArtistProperty, out albumArtist))
                            {
                                albumArtist = new StandardArtist(albumArtistProperty);
                                artists.Add(albumArtist.Name, albumArtist);
                            }

                            IGenre genre;
                            if (!genres.TryGetValue(genreProperty, out genre))
                            {
                                genre = new StandardGenre(genreProperty);
                                genres.Add(genre.Name, genre);
                            }

                            IAlbum album;
                            if (!albums.TryGetValue(albumNameProperty, out album))
                            {
								album = new AndroidAlbum(mArtResolver, new StandardSongCollection(), albumNameProperty, albumArtist, genre, albumArtUri);
                                albums.Add(album.Name, album);
                                albumList.Add(album);
                            }

							var song = new AndroidSong(album, artist, genre, titleProperty, duration, assetUri);
							song.Initialize();
                            song.Album.Songs.Add(song);
                            songList.Add(song);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("MediaLibrary exception: " + e.Message);
                        }
                }

                musicCursor.Close();
            }

            albumCollection = new StandardAlbumCollection(albumList);
            songCollection = new StandardSongCollection(songList);
        }

        public IAlbumCollection GetAlbums()
        {
            return albumCollection;
        }

        public ISongCollection GetSongs()
        {
            return songCollection;
        }
    }
}
