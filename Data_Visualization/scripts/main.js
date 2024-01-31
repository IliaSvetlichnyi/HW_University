// Function to parse each row
function parseRow(d) {
    return {
        release_year: +d.release_year, // convert to number
        release_month: +d.release_month, // convert to number
        genre: d.genre,
        director: d.director,
        budget: +d.budget, // convert to number
        revenues: +d.revenues, // convert to number
        ratings_A: +d.ratings_A, // convert to number
        ratings_B: +d.ratings_B, // convert to number
        ratings_C: +d.ratings_C, // convert to number
        profits: +d.revenues - +d.budget, // calculate profits
        success: (+d.revenues - +d.budget) > 0 // determine success
    };
}

// Load the dataset
async function loadData() {
    try {
        let data = await d3.csv("data/movies_mock.csv", parseRow);
        console.log("Data loaded:", data);

        // Call the processData function
        processData(data);
    } catch (error) {
        console.error("Error loading the data: ", error);
    }
}



// Process the data
function processData(data) {
    console.log("Processing data...");

    // Unique genres of movies
    let genres = new Set();
    data.forEach(movie => genres.add(movie.genre));
    console.log("Unique genres:", Array.from(genres));

    // Number of unique directors
    let directors = new Set();
    data.forEach(movie => directors.add(movie.director));
    console.log("Unique directors:", directors.size);

    // Total sum of revenue
    let totalRevenue = data.reduce((sum, movie) => sum + movie.revenues, 0);
    console.log("Total revenue:", totalRevenue);

    // Movies released between 2012 and 2014 (included)
    let movies2012To2014 = data.filter(movie => movie.release_year >= 2012 && movie.release_year <= 2014);
    console.log("Movies released between 2012 and 2014:", movies2012To2014.length);

    // Average rating on website A for comedy movies
    let comedyMovies = data.filter(movie => movie.genre === "Comedy");
    let avgRatingAComedy = comedyMovies.reduce((sum, movie) => sum + movie.ratings_A, 0) / comedyMovies.length;
    console.log("Average rating on website A for comedy movies:", avgRatingAComedy);

    // Industry profit comparison before and after 2015
    let profitsBefore2015 = data.filter(movie => movie.release_year <= 2015).reduce((sum, movie) => sum + (movie.revenues - movie.budget), 0);
    let profitsAfter2015 = data.filter(movie => movie.release_year > 2015).reduce((sum, movie) => sum + (movie.revenues - movie.budget), 0);
    let moreProfitPeriod = profitsBefore2015 > profitsAfter2015 ? "before 2015" : "after 2015";
    console.log("The industry made more profit", moreProfitPeriod);

    // Average budget of drama movies with a rating above 70% on website C
    let dramaMoviesHighRatingC = data.filter(movie => movie.genre === "Drama" && movie.ratings_C > 70);
    let avgBudgetDramaHighRatingC = dramaMoviesHighRatingC.reduce((sum, movie) => sum + movie.budget, 0) / dramaMoviesHighRatingC.length;
    console.log("Average budget of drama movies with a rating above 70% on website C:", avgBudgetDramaHighRatingC);

}

// Call the loadData function
loadData();




