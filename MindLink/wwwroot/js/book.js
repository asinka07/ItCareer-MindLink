function initializeBook() {
    const prevBtn = document.querySelector("#prev-btn");
    const nextBtn = document.querySelector("#next-btn");
    const book = document.querySelector("#book");

    if (!book || !prevBtn || !nextBtn) { 
        console.warn("Book elements not found — skipping initialization."); 
        return;
    }

    // взимаме всички страници
    const papers = Array.from(document.querySelectorAll(".book .paper"));
    const numOfPapers = papers.length;

    papers.forEach((paper, index) => {
        paper.style.zIndex = numOfPapers - index;
    });

    let currentLocation = 0; // започваме с първата страница не обърната

    prevBtn.addEventListener("click", goPrevPage);
    nextBtn.addEventListener("click", goNextPage);

    function openBook() {
        book.style.transform = "translateX(50%)";
        prevBtn.style.transform = "translateX(-180px)";
        nextBtn.style.transform = "translateX(180px)";
    }

    function closeBook(isAtBeginning) {
        if (isAtBeginning) {
            book.style.transform = "translateX(0%)";
        } else {
            book.style.transform = "translateX(100%)";
        }
        prevBtn.style.transform = "translateX(0px)";
        nextBtn.style.transform = "translateX(0px)";
    }

    function goNextPage() {
        if (currentLocation < numOfPapers) {
            const paper = papers[currentLocation];
            paper.classList.add("flipped");
            paper.style.zIndex = currentLocation;

            currentLocation++;

            if (currentLocation === numOfPapers) {
                closeBook(false);
            } else {
                openBook();
            }
        }
    }

    function goPrevPage() {
        if (currentLocation > 0) {
            currentLocation--;

            const paper = papers[currentLocation];
            paper.classList.remove("flipped");
            paper.style.zIndex = numOfPapers - currentLocation;

            if (currentLocation === 0) {
                closeBook(true);
            } else {
                openBook();
            }
        }
    }
}