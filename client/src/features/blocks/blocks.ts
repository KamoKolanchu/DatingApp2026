import { Component, ElementRef, inject, OnInit, signal } from '@angular/core';
import { BlocksService } from '../../core/services/blocks-service';
import { PaginatedResult } from '../../types/pagination';
import { Member } from '../../types/member';
import { Paginator } from '../../shared/paginator/paginator';
import { BlockReasonModalService } from '../../core/services/block-reason-modal-service';

@Component({
  selector: 'app-blocks',
  imports: [Paginator],
  templateUrl: './blocks.html',
  styleUrl: './blocks.css',
})
export class Blocks implements OnInit {
  protected blocksService = inject(BlocksService);
  protected pageNumber = 1;
  protected pageSize = 10;
  protected paginatedBlockedMembers = signal<PaginatedResult<Member> | null>(null);
  protected blockReasonModal = inject(BlockReasonModalService);

  ngOnInit(): void {
    this.loadBlockedMembers();
  }

  loadBlockedMembers() {
    this.blocksService.getBlockedMembers(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.paginatedBlockedMembers.set(response);
      },
    });
  }

  unblock(id: string) {
    this.blocksService.unblock(id).subscribe({
      next: () => {
        const current = this.paginatedBlockedMembers();
        if (current?.items) {
          this.paginatedBlockedMembers.update((prev) => {
            if (!prev) return null;
            const newItems = prev.items.filter((x) => x.id !== id) || [];
            return {
              items: newItems,
              metadata: prev.metadata,
            };
          });
        }
      },
    });
  }

 async editReason(block: Member) {

}

  onPageChange(event: { pageNumber: number; pageSize: number }) {
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageNumber;
    this.loadBlockedMembers();
  }
}
